# ICS Project: Game Launcher

## Progress: Fáze 3 – MAUI frontend, data binding

## Introduction

This is a semester project for the ICS course. It is a game launcher application inspired by platforms like Steam and Epic Games. Users can browse game titles available on the platform, manage personal libraries, and track playtime per title.

The application is built with a strong focus on clean architecture, object-oriented design, and database integration using Entity Framework Core.

## Architecture & Technologies

The solution follows a multi-project layered architecture to strictly separate concerns:

| Project | Role |
|---|---|
| `GameLib.App` | .NET MAUI frontend — Views and ViewModels |
| `GameLib.BL` | Business Logic — Facades, model mappers, DTOs |
| `GameLib.DAL` | Data Access Layer — EF Core, entities, migrations, repositories |
| `GameLib.BL.Tests` | Integration and unit tests for the BL/facade layer |
| `GameLib.Common.Tests` | Shared test infrastructure and helpers |
| `GameLib.Tests` | DAL-level tests |

**Key technologies:**
- .NET 10.0
- .NET MAUI (cross-platform UI)
- Entity Framework Core 10 — Code First, SQLite
- CommunityToolkit.Mvvm — ObservableObject, RelayCommand, source generators
- xUnit — automated testing

All filtering, searching, and sorting are executed at the database level via EF Core `IQueryable` — no in-memory operations.

## Domain Model

The application manages the following core entities:

- **User** — has a username, email, optional first/last name, and owns one or more Libraries
- **Library** — belongs to a User, contains a collection of Games
- **Game** — has a name, description, PEGI rating, image, studio, categories, and playtime timers
- **Studio** — a game developer/publisher; one Studio can have many Games
- **Category** — a genre/tag (enum-based); Games can belong to multiple Categories
- **Timer** — tracks a playtime session (duration + date) for a specific Game

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022+ or JetBrains Rider
- EF Core CLI tools:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### Running the Application

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore solution.slnx
   ```

3. **Apply database migrations:**

   The database is created automatically on first launch. If you need to apply migrations manually, run the following from the repository root:
   ```bash
   dotnet ef database update --project GameLib.DAL --startup-project GameLib.DAL
   ```
   This will create a local SQLite database file (`gamelib.db`) in the application's working directory.

4. **Build and run the MAUI app:**
   ```bash
   dotnet build GameLib.App/GameLib.App.csproj
   ```
   Then launch via Visual Studio / Rider using the desired target platform (Windows, Android, etc.).

### Running Tests

```bash
dotnet test solution.slnx
```

Or per project:
```bash
dotnet test GameLib.BL.Tests/GameLib.BL.Tests.csproj
dotnet test GameLib.Tests/GameLib.DAL.Tests.csproj
```

## Features

- Browse all available game titles with name, image, PEGI rating, and studio
- View full game detail including description, categories, and total playtime
- User selection on app launch — switch active user at any time without authentication
- Create, edit, and delete users, libraries, and games (full CRUD)
- Add and remove games from personal libraries
- Filter games by name, PEGI age rating, and studio
- Sort game lists by name and rating (ascending/descending)
- All filtering and sorting executed directly in the database

## Project Structure

```
solution.slnx
├── GameLib.App/            # MAUI application (Views, ViewModels, MauiProgram.cs)
├── GameLib.BL/             # Business logic (Facades, Mappers, Models/DTOs)
│   ├── Facades/
│   ├── Mappers/
│   └── Models/
├── GameLib.DAL/            # Data access (DbContext, Entities, Migrations, Repositories)
│   ├── Entities/
│   ├── Enums/
│   ├── Factories/
│   ├── Migrations/
│   ├── Repositories/
│   └── UnitOfWork/
├── GameLib.BL.Tests/
├── GameLib.Common.Tests/
└── GameLib.Tests/
```

## Notes

- The application does not implement authentication or authorization, as per the project specification. User identity is selected from the UI and can be switched at any time.
- Multiple simultaneous instances of the application are supported — all instances share the same SQLite file and data changes are reflected across instances upon reload.
- The project language for identifiers, class names, and comments is English, as required by the course conventions.