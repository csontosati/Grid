---
name: grid-architecture
description: Architectural standards, domain model, layer contracts (DAL, BL, App), and development guidelines for the Grid project. Use whenever designing or modifying entities, DbContext, repositories, unit of work, facades, DTOs/models, seeding, security package pins, or inspecting contract-defining files.
---

# Grid Architecture & Domain Guide

Architectural reference, domain model conventions, and layer boundaries for the Grid game launcher project.

## 1. High-Level Architecture

The solution adheres to a strict 3-tier separation of concerns:

```
[Presentation Layer]  GameLib.App (MAUI, being migrated to Avalonia)
       │
       ▼
[Business Logic]      GameLib.BL (Facades + DTO Models)
       │
       ▼
[Data Access Layer]   GameLib.DAL (EF Core SQLite + Repository / Unit of Work)
```

Each layer provides an installer class (`DALInstaller.cs`, `BLInstaller.cs`, `AppInstaller.cs`) that registers its internal services, factories, and facades into Microsoft Dependency Injection (`IServiceCollection`).

---

## 2. Domain Model & Relations

Entities are defined in `GameLib.DAL/Entities/`:

| Entity | Description | Relationships |
|---|---|---|
| `User` | Application user profile | `1 : N` Library |
| `Library` | User's game library | `N : 1` User, `N : N` Game (via join) |
| `Game` | Game title entry | `N : N` Library, `N : N` Category, `1 : N` Timer, `N : 1` Studio |
| `Studio` | Development/publishing studio | `1 : N` Game |
| `Category` | Game genre/category | `N : N` Game |
| `Timer` | Playtime tracking record | `N : 1` Game |

### Enums (`GameLib.DAL/Enums/`)
- `GameCategory`: Action, Adventure, RPG, Strategy, Indie, etc.
- `Pegi`: Age rating categories (Pegi3, Pegi7, Pegi12, Pegi16, Pegi18).

---

## 3. Data Access Layer (`GameLib.DAL`)

- **Database**: SQLite via Entity Framework Core (Code First).
- **DbContext**:
  - `GameLibDbContext`: Core context declaring DbSets and fluent relationship configurations (`OnModelCreating`).
  - `DbContextSqLiteFactory`: Runtime factory instantiated by DI.
  - `DesignTimeDbContextFactory`: Design-time factory used by `dotnet ef` CLI tools.
- **Repository & Unit of Work**:
  - Generic `Repository<TEntity>` implementing `IRepository<TEntity>`.
  - Throws `EntityNotFoundException` when entities cannot be resolved.
  - `UnitOfWork` orchestrates transaction lifecycles and commits across repositories.
- **Mappers (`GameLib.DAL/Mappers/`)**:
  - `*EntityMapper.cs`: Map database entities to internal DAL representations.
- **Migrations & Seeding**:
  - `DbMigrator.cs` (`IDbMigrator`): Applies pending migrations automatically at application startup.
  - `DbSeeder.cs` (`IDbSeeder`): Seeds starter games, users, studios, and libraries from `GameLib.DAL/Seeds/`.

### ⚠️ Security Pins & NuGet Quirks in DAL
- **`System.Security.Cryptography.Xml`**: Explicitly pinned to `10.0.11`.
  - *Warning*: Do not downgrade to `10.0.5` — `10.0.5` is also flagged as vulnerable in NuGet security advisories.
- **`SQLitePCLRaw.lib.e_sqlite3`**: Currently suppressed via `<NuGetAuditSuppress>` in `GameLib.DAL.csproj` due to an upstream EF Core issue without a patched 2.1.x release.

---

## 4. Business Logic Layer (`GameLib.BL`)

- **DTOs / Models (`GameLib.BL/Models/`)**:
  - Distinct DTOs for list and detail views (e.g., `GameListModel` vs `GameDetailModel`, `UserListModel` vs `UserDetailModel`).
  - Base class: `ModelBase`.
- **Model Mappers (`GameLib.BL/Mappers/`)**:
  - Handle conversions between DAL entities and BL DTO models (`GameModelMapper`, `UserModelMapper`, etc.).
- **Facades (`GameLib.BL/Facades/`)**:
  - Public interface consumed by the presentation layer (`GameFacade`, `LibraryFacade`, `UserFacade`).
  - Encapsulate CRUD, queries, and sorting/filtering.
  - **Sorting switch convention**: Double-check sorting logic pattern in facades. For example, `GameFacade.ApplyOrder` must support both ascending and descending cases (`name`, `name_desc`, `age`, `age_desc`) rather than falling through to default ordering.

---

## 5. Presentation Layer (`GameLib.App`)

- **ViewModels**: Built on `CommunityToolkit.Mvvm` (`ObservableObject`, `[ObservableProperty]`, `[RelayCommand]`).
  - Inherit from `ViewModelBase`.
  - ViewModels communicate via weak messages (`CommunityToolkit.Mvvm.Messaging`).
  - **Decoupled from MAUI**: ViewModels and Messages are 100% portable to Avalonia.
- **Services**:
  - `INavigationService`: Abstraction for view switching and parameter passing.
  - `IAlertService`: Abstraction for displaying dialogs and confirmations.
  - `IMessengerService`: Abstraction for pub/sub messaging.

---

## 6. Contract-Defining Files to Consult First

When exploring or modifying functionality, inspect these files first:
1. `solution/GameLib.DAL/GameLibDbContext.cs` — Schema definitions and relationships.
2. `solution/GameLib.BL/Facades/*.cs` — Business logic and query API surface.
3. `solution/GameLib.BL/Models/*.cs` — DTO shapes and data contracts.
4. `solution/GameLib.App/ViewModels/` — UI behavior and screen states.
5. `*Installer.cs` — Dependency injection registrations and wiring across layers.

---

## 7. Git & Development Workflow

- **Branch**: Work directly off `main`. `main` is complete and contains the full application.
- **Branch Protection**: GitHub Actions CI builds and runs tests for DAL and BL on pull requests. Direct pushes to `main` are restricted.
- **Local Tooling**: JetBrains Rider (recommended) or VS Code on native Windows PowerShell.

