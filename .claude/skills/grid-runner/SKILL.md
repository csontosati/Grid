---
name: grid-runner
description: Build, run, test, and manage EF Core database migrations for the Grid game launcher project on Windows. Use whenever building the solution, running GameLib.App, executing migrations, running xUnit tests, or troubleshooting common build/runtime issues like exit code 9009 or cache corruption.
---

# Grid Runner & Local Dev Workflow

Instructions and command sequences for developing, building, running, and testing the Grid game launcher on Windows.

## 1. Prerequisites & Environment

- **OS / Shell**: Native Windows PowerShell (run at the repository root `C:\Users\...\Grid`).
  - **Do NOT run Windows build targets from WSL** — the Windows build target (`net10.0-windows10.0.19041.0`) requires native Windows tooling.
- **SDK**: .NET 10 SDK.
- **Workloads**:
  ```powershell
  dotnet workload install maui
  ```
- **EF Core CLI**:
  ```powershell
  dotnet tool install --global dotnet-ef
  # If already installed or acting up:
  dotnet tool update --global dotnet-ef
  ```

---

## 2. Quick Reference (Full Sequence)

```powershell
# Ensure working directory is repo root
git checkout main
git pull origin main

# 1. Update database
dotnet ef database update --project solution\GameLib.DAL\GameLib.DAL.csproj --startup-project solution\GameLib.DAL\GameLib.DAL.csproj

# 2. Build the app
cd solution\GameLib.App
dotnet build -f net10.0-windows10.0.19041.0

# 3. Launch the app executable directly
cd bin\Debug\net10.0-windows10.0.19041.0\win-x64
.\GameLib.App.exe
```

---

## 3. Database Migrations (EF Core)

> [!IMPORTANT]
> **Always run EF Core CLI from the repository root** pointing both `--project` and `--startup-project` at `solution\GameLib.DAL\GameLib.DAL.csproj`.
>
> **Never** run `dotnet ef` inside `GameLib.App` or target `GameLib.App` as the startup project. Because `GameLib.App` has multi-targeting (Android, iOS, MacCatalyst, Windows), the EF CLI will fail or get confused. `GameLib.DAL` provides its own `DesignTimeDbContextFactory` to serve as its own startup project.

### Apply Migrations
```powershell
dotnet ef database update --project solution\GameLib.DAL\GameLib.DAL.csproj --startup-project solution\GameLib.DAL\GameLib.DAL.csproj
```
Expected output ends with `Done.` or `No migrations were applied. The database is already up to date.`

### Add a New Migration
```powershell
dotnet ef migrations add <MigrationName> --project solution\GameLib.DAL\GameLib.DAL.csproj --startup-project solution\GameLib.DAL\GameLib.DAL.csproj
```

---

## 4. Building and Running `GameLib.App` (Path A vs Path B)

> [!CAUTION]
> **AI Agent Background Execution Quirk**:
> Automated agent tools run in headless background shells. Windows security prevents background subprocesses from rendering GUI windows on the user's interactive monitor (`MainWindowHandle` remains `0`). Therefore, **agents must build the app, and the user must launch the executable**.

### Path A: User Manual Execution
Run directly in an interactive Windows PowerShell:
```powershell
cd solution\GameLib.App
dotnet build -f net10.0-windows10.0.19041.0
cd bin\Debug\net10.0-windows10.0.19041.0\win-x64
.\GameLib.App.exe
```

### Path B: AI Agent Assisted (Recommended when pairing with an LLM)
1. **Agent compiles the app**:
   ```powershell
   dotnet build solution\GameLib.App\GameLib.App.csproj -f net10.0-windows10.0.19041.0
   ```
2. **User launches the executable**:
   The agent reports compilation status and asks the user to run:
   ```powershell
   cd C:\Users\csont\Documents\GitHub\Grid\solution\GameLib.App\bin\Debug\net10.0-windows10.0.19041.0\win-x64; .\GameLib.App.exe
   ```
   *(Or double-click `GameLib.App.exe` in File Explorer).*


---

## 5. Testing

Unit tests cover `GameLib.DAL` and `GameLib.BL` using xUnit. The App project does not have unit tests.

### Run All Tests
```powershell
dotnet test solution\solution.slnx
```

### Run Specific Test Projects
```powershell
dotnet test solution\GameLib.Tests\GameLib.Tests.csproj
dotnet test solution\GameLib.BL.Tests\GameLib.BL.Tests.csproj
```

---

## 6. Troubleshooting & Known Quirks

### ⚠️ Exit Code 9009 on `dotnet build -t:Run`
**Issue**: Running `dotnet build -t:Run -f net10.0-windows10.0.19041.0` fails with:
`MSB3073: The command "...\GameLib.App.exe " exited with code 9009.`
**Cause**: The MSBuild `-t:Run` wrapper fails to launch the WinUI/AppSDK executable in this environment, even though compilation succeeded.
**Solution**: Never use `-t:Run`. Always split build and launch into two separate steps (build, then run `.\GameLib.App.exe`).

### ⚠️ Warnings: NU1903 SQLitePCLRaw
**Issue**: `NU1903` warnings regarding `SQLitePCLRaw.lib.e_sqlite3` transitive packages.
**Status**: Expected and tracked. Suppressed via `NuGetAuditSuppress` in `GameLib.DAL.csproj`. Safe to ignore during local runs.

### ⚠️ Stale `obj/` / `bin/` Caches (NETSDK1064 / Missing Packages)
If weird package-not-found errors occur after dependency modifications:
```powershell
Get-ChildItem -Recurse -Directory -Include bin,obj | Remove-Item -Recurse -Force
dotnet nuget locals all --clear
dotnet restore solution\solution.slnx
```

