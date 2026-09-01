# Grid — Repo Roadmap (for LLM context)

Repo: github.com/csontosati/Grid — .NET MAUI "Game Launcher" (ICS course project, Steam-like).
IMPORTANT: `main` only has the backend (Phase 2 snapshot). The **full graded MVP** lives on
branch **`3.submission`** — use that branch for any real work, not main.

## Architecture (layered, MVVM, EF Core + SQLite)
GameLib.App (MAUI, MVVM) → GameLib.BL (Facades/DTOs) → GameLib.DAL (EF Core repos/UoW, SQLite)
Each layer has an `*Installer.cs` (AppInstaller / BLInstaller / DALInstaller) registering its
services into DI, wired up in `MauiProgram.cs`. xUnit tests cover DAL + BL.

## Domain model
Entities (DAL/Entities): User, Library, Game, Studio, Category, Timer.
Relations: User 1–N Library; Library N–N Game; Game N–N Category; Game 1–N Timer; Studio 1–N Game.
Enums: GameCategory, Pegi (age rating).

## File map

### GameLib.DAL — data access
- Entities/*.cs, Enums/*.cs — schema
- GameLibDbContext.cs — DbSets + relations (OnModelCreating)
- Factories/ — DbContextSqLiteFactory (runtime), DesignTimeDbContextFactory (EF CLI)
- Repositories/ (generic Repository+IRepository, EntityNotFoundException), UnitOfWork/ (UoW pattern)
- Mappers/*EntityMapper.cs — entity <-> other repr.
- Migrations/ — InitialMigration + snapshot
- Migrator/DbMigrator.cs + IDbMigrator.cs — applies migrations on startup
- Seeds/ — DbSeeder.cs (+IDbSeeder) and per-entity seed data (Category/Game/Library/Studio/Timer/User) — seeds dev DB
- DALInstaller.cs — registers DbContext factory, repos, UoW, migrator, seeder in DI

### GameLib.BL — business logic
- Models/*.cs — DTOs (GameDetail/List, LibraryDetail/List, UserDetail/List, TimerModel, ModelBase)
- Mappers/*ModelMapper.cs — Entity -> DTO mapping
- Facades/ — GameFacade, LibraryFacade, UserFacade (+BaseFacade, IFacade, IGameFacade) — CRUD/query API for the App layer, DB-level filter/sort
- BLInstaller.cs, BussinesLogic.cs — DI registration for BL layer

### GameLib.App — MAUI frontend (MVVM Toolkit)
- MauiProgram.cs, AppInstaller.cs, Extensions/ServiceCollectionExtension.cs — app bootstrap & DI
- App.xaml(.cs), AppShell.xaml(.cs) — app root & Shell navigation
- ViewModels/ — AppShellViewModel, GameListViewModel, GameDetailViewModel, GameAddViewModel, LibraryListViewModel, UserListViewModel, UserAddViewModel, UserSettingsViewModel (+ViewModelBase)
- Views/ — DiscoverView, GameAddView, GameDetailView, GameEditView, LibraryView, LibraryGameDetailView, SignUpView, UserSelectionView, UserSettingsView (+ContentPageBase)
- Messages/ — MVVM Toolkit weak-messages for cross-VM events (GameAdded/Deleted/Updated/Selected, UserAdded/Deleted/Updated/Selected, LibrarySelected, LibraryGameDeleted, NewUserLibrary)
- Services/ — NavigationService, AlertService, MessengerService (+interfaces) — nav, alerts, pub/sub
- Converters/GameCategoryToStringConverter.cs; Models/RouteModel.cs
- Resources/ — Styles (Colors.xaml, Styles.xaml), Fonts, AppIcon, Splash, Texts (GameCategoryTexts .resx, localized enum display)
- Platforms/ — Android, iOS, MacCatalyst, Windows platform entry points (standard MAUI scaffolding)

### Tests (xUnit)
- GameLib.Tests — DAL: CategoryTests, GameTests, LibraryTests, StudioTests, TimerTests, UserTests, DbContextTestsBase
- GameLib.BL.Tests — GameFacadeTests, LibraryFacadeTests, UserFacadeTests, FacadeTestsBase
- GameLib.Common.Tests — DeepAssert.cs + Seeds/ (shared test fixture data, separate from DAL/Seeds which is app runtime seed data)

### Root
- solution/solution.slnx — solution file
- azure-pipelines.yml — CI (build/test)
- README.md — intro/stack/setup

## Branches (for reference)
`main` = backend-only snapshot. `3.submission` = **graded MVP, full app** (use this one).
Other feature branches exist (DALTesting, Discover, GameDetail, Library, LibraryViewModelImplementation,
New_Game, Views, viewModels-initial, review1/2, patch, init*, azure-pipelines) — mostly merged into 3.submission already; check before reusing.

## For future LLM sessions
1. `git checkout 3.submission` (or fetch it) — do not assume main is current.
2. Contract-defining files to read first: GameLibDbContext.cs (schema), Facades/*.cs (BL API surface),
   Models/*.cs (DTO shape), AppShell.xaml(.cs) + ViewModels/ (navigation/UI structure), *Installer.cs files (DI wiring/how layers connect).
3. Load this file instead of re-scanning the full tree.