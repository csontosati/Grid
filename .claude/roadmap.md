# Grid — Repo Roadmap (for LLM context)

Repo: github.com/csontosati/Grid — Game Launcher (Steam-like), ICS course project.
Status: MVP graded and complete. Currently **mid-migration away from .NET MAUI to Avalonia UI**
(MAUI has no Linux desktop target; team wants Linux support). Read this whole file before
touching anything — a lot changed recently and previous LLM sessions' assumptions may be stale.

## Current architecture (as of last session)
```
GameLib.App   → .NET MAUI frontend, MVVM (CommunityToolkit.Mvvm) — BEING REPLACED, see below
GameLib.BL    → Business logic: Facades + DTOs (Models), mapped from entities — stays as-is
GameLib.DAL   → EF Core (Code First) + SQLite, Repository/UnitOfWork pattern — stays as-is
```
Each layer has an `*Installer.cs` (`AppInstaller` / `BLInstaller` / `DALInstaller`) registering
its services into DI, wired up in `MauiProgram.cs`. xUnit tests cover DAL + BL only (App has no
tests, not worth adding given the pending rewrite).

`main` now contains the **full app** (this was NOT true a few sessions ago — an earlier merge
attempt silently missed `solution/GameLib.App`; it was manually re-checked-out and fixed).
There is no longer a need to check other branches for "the real code" — `main` is current.

## Domain model
Entities (DAL/Entities): User, Library, Game, Studio, Category, Timer.
Relations: User 1–N Library; Library N–N Game; Game N–N Category; Game 1–N Timer; Studio 1–N Game.
Enums: GameCategory, Pegi (age rating).

## File map

### GameLib.DAL — data access (UNCHANGED by the Avalonia migration)
- Entities/*.cs, Enums/*.cs — schema
- GameLibDbContext.cs — DbSets + relations (OnModelCreating)
- Factories/ — DbContextSqLiteFactory (runtime), DesignTimeDbContextFactory (EF CLI design-time;
  DAL can act as its own `--startup-project` for `dotnet ef` commands, no need to involve App)
- Repositories/ (generic Repository+IRepository, EntityNotFoundException), UnitOfWork/ (UoW pattern)
- Mappers/*EntityMapper.cs — entity <-> other repr.
- Migrations/ — InitialMigration + snapshot
- Migrator/DbMigrator.cs + IDbMigrator.cs — applies migrations on startup
- Seeds/ — DbSeeder.cs (+IDbSeeder) and per-entity seed data — seeds dev DB with starter data
- DALInstaller.cs — registers DbContext factory, repos, UoW, migrator, seeder in DI
- **Known issue, fixed**: NU1903 vulnerabilities were present — `System.Security.Cryptography.Xml`
  pinned to `10.0.11` (NOT 10.0.5 — that version is *also* on the vulnerable list, easy mistake to
  repeat); `SQLitePCLRaw.lib.e_sqlite3` has no patched 2.1.x release yet, suppressed via
  `NuGetAuditSuppress` in the csproj pointing at the upstream EF Core issue tracking it. Revisit
  periodically — check if EF Core has bumped the transitive reference.

### GameLib.BL — business logic (UNCHANGED by the Avalonia migration)
- Models/*.cs — DTOs (GameDetail/List, LibraryDetail/List, UserDetail/List, TimerModel, ModelBase)
- Mappers/*ModelMapper.cs — Entity -> DTO mapping
- Facades/ — GameFacade, LibraryFacade, UserFacade (+BaseFacade, IFacade, IGameFacade) — CRUD/query
  API for the App layer, DB-level filter/sort. **Bug found+fixed**: `GameFacade.ApplyOrder`'s
  switch was missing `age_desc` (fell through to unordered `_ => query`) — added, plus `name_desc`
  for symmetry. Worth double-checking other facades for the same missing-case pattern.
- BLInstaller.cs, BussinesLogic.cs — DI registration for BL layer

### GameLib.App — MAUI frontend, TO BE REPLACED (do not invest further here)
- MauiProgram.cs, AppInstaller.cs, Extensions/ServiceCollectionExtension.cs — app bootstrap & DI
- App.xaml(.cs), AppShell.xaml(.cs) — app root & Shell navigation
- ViewModels/ — AppShellViewModel, GameListViewModel, GameDetailViewModel, GameAddViewModel,
  LibraryListViewModel, UserListViewModel, UserAddViewModel, UserSettingsViewModel (+ViewModelBase)
  — **these survive the migration essentially unchanged**, pure MVVM Toolkit C#, no MAUI dependency
- Views/ — DiscoverView, GameAddView, GameDetailView, GameEditView, LibraryView,
  LibraryGameDetailView, SignUpView, UserSelectionView, UserSettingsView (+ContentPageBase)
  — full rewrite required (`.xaml` → Avalonia `.axaml`, `ContentPage` → `UserControl`/`Window`)
- Messages/ — MVVM Toolkit weak-messages (GameAdded/Deleted/Updated/Selected,
  UserAdded/Deleted/Updated/Selected, LibrarySelected, LibraryGameDeleted, NewUserLibrary)
  — **survive unchanged**, plain C#, no MAUI dependency
- Services/ — NavigationService, AlertService, MessengerService (+interfaces) — interfaces survive,
  implementations need rewriting (NavigationService is wired to MAUI Shell, which has no Avalonia
  equivalent — biggest single piece of migration work, see below)
- Converters/GameCategoryToStringConverter.cs; Models/RouteModel.cs — near-identical port
- Resources/ — Styles (Colors.xaml, Styles.xaml → Avalonia Styles/ControlTheme), Fonts, AppIcon,
  Splash (drop, no mobile targets in Avalonia desktop), Texts (.resx localization — reusable as-is)
- Platforms/ — Android/iOS/MacCatalyst/Windows — **delete entirely**, Avalonia doesn't use MAUI's
  per-platform head folders

### Tests (xUnit)
- GameLib.Tests — DAL: CategoryTests, GameTests, LibraryTests, StudioTests, TimerTests, UserTests, DbContextTestsBase
- GameLib.BL.Tests — GameFacadeTests, LibraryFacadeTests, UserFacadeTests, FacadeTestsBase
- GameLib.Common.Tests — DeepAssert.cs + Seeds/ (shared test fixture data)

### Root / infra
- solution/solution.slnx — solution file
- .github/workflows/ci.yml — GitHub Actions CI, builds+tests DAL+BL only (App excluded, needs
  platform workloads unsuited to CI). **Branch protection is on for `main`** — no direct pushes,
  PR + passing CI required.
- .azure/azure-pipelines.yml — legacy Azure Pipelines config, may be dead weight; unconfirmed
  whether Azure DevOps is still actively wired to this repo
- .claude/roadmap.md — **this file**
- README.md — rewritten in English, describes MVP status, has a `## Usage` TODO section pending

## Branches
`main` is current and complete — no need to check other branches for "the real code" anymore.
Already deleted (fully merged, redundant): DALTesting, GameDetail, New_Game, Views, init,
init_project_structure, review1, review2, add-ci-workflow.
Still present, unreviewed, believed superseded/abandoned (verify via GitHub compare view before
deleting): Discover, Library, LibraryViewModelImplementation, viewModels-initial, patch,
azure-pipelines. `3.submission` also likely still present — was the source of the App merge into
main, now redundant, safe to delete once `main` is confirmed solid.

## Active migration: MAUI → Avalonia UI
Decision made: rebuild `GameLib.App` in Avalonia (not Blazor/Uno/Photino) specifically to
minimize rewrite effort, since it's XAML+MVVM like MAUI. Full comparison written up separately
(not yet committed to repo — ask if it needs to be re-created).

**Plan**: pilot the port on one small view first (`UserSettingsView` or `SignUpView`) to nail the
Shell-replacement navigation pattern (Avalonia has no Shell equivalent — hand-roll via
`ViewLocator` + `ContentControl` swap, or a community nav package) and the DI bootstrap
(`Program.cs`/`App.axaml.cs` replacing `MauiProgram.cs`, reusing `DALInstaller`/`BLInstaller`
as-is). Once that pattern works, port remaining views mechanically against the same template.

**Not yet started**: no `GameLib.Avalonia` (or similarly named) project exists yet in the repo.

## Local dev environment notes
- Windows dev via VS Code (not Visual Studio, disliked) + WSL for git/bash work; .NET SDK
  separately needed inside WSL if using it there (`sudo snap install dotnet --classic`)
- IDE decision made: **JetBrains Rider**, free for non-commercial use, for both team members
  (confirm this is still current policy if it's been a while)
- To run DB migrations, always specify project explicitly to avoid EF CLI getting confused by
  `GameLib.App`'s multi-targeting:
  `dotnet ef database update --project solution\GameLib.DAL\GameLib.DAL.csproj --startup-project solution\GameLib.DAL\GameLib.DAL.csproj`
- If `NETSDK1064`/package-not-found errors appear after changing package versions, it's usually a
  stale `obj/`/`bin/` cache — `Get-ChildItem -Recurse -Directory -Include bin,obj | Remove-Item -Recurse -Force`
  then `dotnet nuget locals all --clear` then `dotnet restore`

## For future LLM sessions
1. Work off `main` — it's current and complete, don't hunt for another branch.
2. If continuing the Avalonia migration: check whether `GameLib.App` still exists (MAUI) or has
   been replaced/renamed yet — don't assume this file is still accurate on that point.
3. Contract-defining files to read first: GameLibDbContext.cs (schema), Facades/*.cs (BL API
   surface), Models/*.cs (DTO shape), ViewModels/ (UI structure/behavior — these survive
   regardless of UI framework), *Installer.cs files (DI wiring/how layers connect).
4. Load this file instead of re-scanning the full tree — but verify a few key facts on arrival
   (does `GameLib.App` still exist, what's on `main`) since this file can go stale between sessions.