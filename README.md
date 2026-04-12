# ICS Project: Game Launcher

## Progress: Fáze 2 – repositáře a mapování

## Introduction
This is a semester project for the ICS course. It is a game launcher application (inspired by platforms like Steam and Epic Games) that allows users to browse various game titles and manage them within their own personal libraries. 

The application is built with a strong focus on clean architecture, object-oriented design, and database integration.

## Architecture & Technologies
The solution follows a multi-project, layered architecture to strictly separate concerns:
* **App (Frontend):** .NET MAUI (Multi-platform App UI) for a cross-platform user interface.
* **BL (Business Logic):** Contains Facades and mapping logic to translate database entities into Data Transfer Objects (DTOs).
* **DAL (Data Access Layer):** Uses Entity Framework Core with a Code First approach to manage data persistence via a local SQLite database. All filtering, searching, and sorting are executed directly at the database level.
* **Tests:** xUnit framework for automated Unit and Integration testing.

## Getting Started

### Prerequisites
* .NET 10.0 SDK (or the version specified by your environment)
* Visual Studio / JetBrains Rider
* EF Core CLI tools (`dotnet tool install --global dotnet-ef`)

### Database Setup
The application uses a local SQLite database