# syntax=docker/dockerfile:1
# ============================================================
# Stage: test
# Restores, builds, and runs the DAL + BL xUnit test suites.
# Usage: docker build --target test .
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS test

ENV DOTNET_NOLOGO=true \
    DOTNET_CLI_TELEMETRY_OPTOUT=true

WORKDIR /src

# Copy project files first — layer cache is invalidated only when these
# change, so a source-only edit doesn't re-trigger a full restore.
COPY solution/GameLib.DAL/GameLib.DAL.csproj                   ./solution/GameLib.DAL/
COPY solution/GameLib.BL/GameLib.BL.csproj                     ./solution/GameLib.BL/
COPY solution/GameLib.Common.Tests/GameLib.Common.Tests.csproj ./solution/GameLib.Common.Tests/
COPY solution/GameLib.Tests/GameLib.DAL.Tests.csproj           ./solution/GameLib.Tests/
COPY solution/GameLib.BL.Tests/GameLib.BL.Tests.csproj        ./solution/GameLib.BL.Tests/

RUN dotnet restore solution/GameLib.Tests/GameLib.DAL.Tests.csproj && \
    dotnet restore solution/GameLib.BL.Tests/GameLib.BL.Tests.csproj

# Copy the full source tree
COPY solution/ ./solution/

RUN dotnet build solution/GameLib.Tests/GameLib.DAL.Tests.csproj \
        --no-restore --configuration Release && \
    dotnet build solution/GameLib.BL.Tests/GameLib.BL.Tests.csproj \
        --no-restore --configuration Release

# Tests run during the image build — the build fails fast if any test fails.
RUN dotnet test solution/GameLib.Tests/GameLib.DAL.Tests.csproj \
        --no-build --configuration Release \
        --logger "console;verbosity=normal" && \
    dotnet test solution/GameLib.BL.Tests/GameLib.BL.Tests.csproj \
        --no-build --configuration Release \
        --logger "console;verbosity=normal"


# ============================================================
# Stage: android-build
# Installs the MAUI Android workload and compiles a Debug APK.
# No emulator or GUI is required — headless compile-validation only.
# Usage: docker build --target android-build .
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS android-build

ENV DOTNET_NOLOGO=true \
    DOTNET_CLI_TELEMETRY_OPTOUT=true

# Java is required by the Android build toolchain (d8, aapt2, etc.)
RUN apt-get update && apt-get install -y --no-install-recommends \
        openjdk-17-jdk-headless \
    && rm -rf /var/lib/apt/lists/*

ENV JAVA_HOME=/usr/lib/jvm/java-17-openjdk-amd64

# Install MAUI Android workload — bundles the Android SDK components
# needed for compilation (platform-tools, build-tools, android platform).
RUN dotnet workload install maui-android --skip-sign-check

WORKDIR /src

# Project files for restore cache
COPY solution/GameLib.DAL/GameLib.DAL.csproj  ./solution/GameLib.DAL/
COPY solution/GameLib.BL/GameLib.BL.csproj    ./solution/GameLib.BL/
COPY solution/GameLib.App/GameLib.App.csproj  ./solution/GameLib.App/

# On Linux, GameLib.App.csproj only targets net10.0-android (the
# ios/maccatalyst/windows conditions are guarded in the csproj).
RUN dotnet restore solution/GameLib.App/GameLib.App.csproj \
        --framework net10.0-android

COPY solution/ ./solution/

# Build Debug APK — no signing setup required.
RUN dotnet build solution/GameLib.App/GameLib.App.csproj \
        --framework net10.0-android \
        --configuration Debug \
        --no-restore


# ============================================================
# Stage: apk-export
# Scratch image containing only the compiled APK.
# Used by the CI "outputs: type=local" step to extract the artifact
# without copying the entire container filesystem.
# ============================================================
FROM scratch AS apk-export
COPY --from=android-build /src/solution/GameLib.App/bin/Debug/net10.0-android/*.apk /apk/
