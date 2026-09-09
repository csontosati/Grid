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
