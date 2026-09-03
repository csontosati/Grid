# Grid — Game Launcher

A game launcher application (inspired by platforms like Steam and Epic Games) that lets users
browse game titles and manage them within their own personal libraries.

Originally built as a semester project for the ICS course, with a focus on clean architecture,
object-oriented design, and database integration.

## Status

MVP complete — full app (frontend + backend), submitted and graded.

## Architecture

Layered, single solution:

```
GameLib.App   → .NET MAUI frontend (MVVM), cross-platform UI
GameLib.BL    → Business logic: Facades + DTOs (Models) mapped from entities
GameLib.DAL   → Data access: EF Core (Code First) + SQLite, Repository/UnitOfWork pattern
```

Each layer registers its own services via an `*Installer.cs` (`AppInstaller`, `BLInstaller`,
`DALInstaller`), wired together in `MauiProgram.cs`. All filtering, searching, and sorting
happens at the database level, not in memory.

Tests use xUnit, covering both `GameLib.DAL` and `GameLib.BL`.

## Domain model

- **User** — owns one or more **Library** entries
- **Library** — a user's collection, linked to many **Game**s
- **Game** — belongs to a **Studio**, tagged with **Category**, tracked via **Timer**
  (play-session tracking)

## Platform support

`GameLib.App` targets:

| Platform | Notes |
|---|---|
| Android | Builds on any OS, including Linux (no Windows/Mac required) |
| iOS / Mac Catalyst | Only builds on non-Linux hosts |
| Windows | Only builds on Windows hosts |

The backend (`GameLib.DAL`, `GameLib.BL`) is plain .NET — no platform restrictions, builds and
tests anywhere .NET 10 runs (including Linux, which is what CI uses).

## Prerequisites

- .NET 10.0 SDK
- Visual Studio / JetBrains Rider (with MAUI workload, if building the app UI)
- EF Core CLI tools: `dotnet tool install --global dotnet-ef`

## Database setup

The application uses a local SQLite database, managed via EF Core Code First migrations.

```bash
cd solution/GameLib.DAL
dotnet ef database update
```

## Running tests

```bash
dotnet test solution/GameLib.Tests/GameLib.DAL.Tests.csproj
dotnet test solution/GameLib.BL.Tests/GameLib.BL.Tests.csproj
```

## CI

GitHub Actions (`.github/workflows/ci.yml`) builds and tests `GameLib.DAL` and `GameLib.BL` on
every push/PR to `main`. `GameLib.App` is excluded from CI since it requires MAUI workloads.

## Usage

_TODO: add usage instructions here._

## 🤖 AI Agent Workflows (Skills)

This repository includes custom agent skills that allow AI coding assistants (like Claude Code) to seamlessly build, run, and understand the project.

If you are pairing with an AI assistant in this repository, you can trigger these workflows directly in the chat using slash commands (e.g., `/grid-runner`):

- **`/grid-runner`**: Automatically handles building the app, running EF Core database migrations, running xUnit tests, and managing the local Windows workflow. *(Use this to quickly start up and run the app!)*
- **`/grid-architecture`**: Teaches the agent the exact 3-tier layer contracts, domain models, relationships, and Git workflow.

*(These skills are automatically discovered by the agent from the `.claude/skills/` directory).*

## License

Apache License 2.0 — see [LICENSE](LICENSE).