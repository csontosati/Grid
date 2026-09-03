---
name: maui-avalonia-migration
description: Guide and execute the migration of Grid's frontend from .NET MAUI to Avalonia UI for Linux and Windows desktop support. Use whenever creating Avalonia views, porting XAML to AXAML, implementing navigation/ViewLocator, adapting services, bootstrapping DI, or migrating GameLib.App components.
---

# MAUI → Avalonia UI Migration Guide

This skill provides step-by-step guidance, component mappings, and architectural patterns for migrating Grid's desktop frontend from **.NET MAUI** (`GameLib.App`) to **Avalonia UI** (`GameLib.Avalonia`).

## Context & Motivation

- **Target Platforms**: Windows + Linux desktop (MAUI does not support Linux desktop; Avalonia natively supports Windows, Linux, and macOS).
- **Guiding Principle**: Smallest rewrite effort from existing code.
  - `GameLib.DAL` and `GameLib.BL` are pure .NET and require **zero changes**.
  - `ViewModels` and `Messages` are pure MVVM Toolkit C# and require **zero changes**.
  - All rework is concentrated in the presentation layer (Views, Navigation, Services, DI bootstrap).

---

## Component Migration Matrix

| MAUI Component | Avalonia Equivalent | Effort | Notes |
|---|---|---|---|
| `ViewModels/*.cs` | Unchanged | None | Pure MVVM Toolkit C#, zero MAUI dependency. |
| `Messages/*.cs` | Unchanged | None | Pure MVVM Toolkit weak-messaging, zero MAUI dependency. |
| `Models/RouteModel.cs`, converters | Unchanged or near-identical | Trivial | Retain route structures and converter logic. |
| `Views/*.xaml` | Avalonia `*.axaml` | Moderate | Same general shape (`<Grid>`, `{Binding}` syntax), but control names/properties differ per view (~9 views). |
| `ContentPage` | `UserControl` or `Window` | Small | Mechanical rename from `ContentPage` to `UserControl` (for pages hosted in navigation). |
| `AppShell.xaml` (Shell navigation) | `ViewLocator` + `ContentControl` swap | High | **Biggest single rewrite piece**. Avalonia lacks built-in Shell. Hand-roll navigation view model or use community nav package. |
| `Services/NavigationService.cs` | Reimplemented | Moderate | Keep `INavigationService` interface contract unchanged; rewrite implementation to navigate via ViewModel/View swapping instead of MAUI Shell. |
| `Services/AlertService.cs` | Reimplemented | Small | Keep `IAlertService` interface contract unchanged; replace MAUI `DisplayAlert` with Avalonia dialog / messagebox APIs. |
| `Resources/Styles/*.xaml` | Avalonia `Styles` / `ControlTheme` | Moderate | Concept maps closely (selector-based styling), but syntax is Avalonia-specific. |
| `*Installer.cs`, DI wiring | `App.axaml.cs` + `Program.cs` | Small | Replaces `MauiProgram.cs`. DI container stays the same; reuse `DALInstaller` and `BLInstaller` as-is. |
| `Platforms/` folder | Delete entirely | None | Not needed; Avalonia desktop is cross-platform without per-OS head folders. |

---

## Porting Strategy: The Pilot Approach

Do **not** attempt to port all views at once. Follow this phased process:

1. **Phase 1: Project Setup & DI Bootstrap**
   - Create the Avalonia project (e.g., `solution/GameLib.Avalonia/GameLib.Avalonia.csproj` referencing `GameLib.BL` and `GameLib.DAL`).
   - Setup `Program.cs` and `App.axaml.cs` configuring `IServiceProvider` with `DALInstaller`, `BLInstaller`, and the new UI installer.
2. **Phase 2: Navigation & ViewLocator Spike**
   - Implement the `ViewLocator` and main window shell containing a `ContentControl` bound to `CurrentViewModel`.
   - Implement the Avalonia-backed `INavigationService`.
3. **Phase 3: Pilot on Smallest View**
   - Port either `UserSettingsView` or `SignUpView`.
   - Verify: DI injection, ViewModel data binding, command execution, and navigation transitions.
4. **Phase 4: Mechanical View Porting**
   - Port the remaining views against the established pilot template:
     - `DiscoverView`, `GameAddView`, `GameDetailView`, `GameEditView`, `LibraryView`, `LibraryGameDetailView`, `UserSelectionView`.
5. **Phase 5: Dialogs & Styles**
   - Implement `AlertService` with Avalonia modal dialogs.
   - Port color palettes and control styles into Avalonia `App.axaml` styles.

---

## Detailed Implementation Patterns

### 1. View & XAML Translation Rules

- **File extension**: Rename `.xaml` to `.axaml` (and `.xaml.cs` to `.axaml.cs`).
- **Root element**:
  ```xml
  <!-- MAUI: -->
  <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui" ...>
  
  <!-- Avalonia: -->
  <UserControl xmlns="https://github.com/avaloniaui"
               xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
               xmlns:vm="using:GameLib.App.ViewModels"
               x:DataType="vm:GameListViewModel" ...>
  ```
- **Layout controls**:
  - Replace `<StackLayout>` or `<VerticalStackLayout>` with `<StackPanel Orientation="Vertical">`.
  - Replace `<HorizontalStackLayout>` with `<StackPanel Orientation="Horizontal">`.
  - `<Grid>` definitions use similar syntax (`ColumnDefinitions="Auto, *"` / `RowDefinitions="Auto, *"`).
- **Lists / Collections**:
  - Replace `CollectionView` with Avalonia `ItemsControl` or `ListBox`.
  - `ItemsSource="{Binding Games}"` syntax remains identical.
- **Compiled Bindings**:
  - Always declare `x:DataType="vm:YourViewModel"` at the root of `.axaml` files for compile-time binding validation and performance.

### 2. Navigation Pattern (Shell Replacement)

In Avalonia, Shell navigation is replaced with a Main Window host and a ViewModel-first navigation pattern:

```csharp
// ViewLocator.cs: Resolves View for ViewModel automatically
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null) return null;
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);
        if (type != null) return (Control)Activator.CreateInstance(type)!;
        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data) => data is ViewModelBase;
}
```

In `MainWindow.axaml`:
```xml
<Window ...>
    <ContentControl Content="{Binding CurrentViewModel}" />
</Window>
```

In `NavigationService.cs`:
Implement `INavigationService` by changing `MainWindowViewModel.CurrentViewModel` or pushing/popping onto a navigation stack.

---

## Pre-Flight Checklist Before Starting Work

- [ ] Check whether `GameLib.Avalonia` already exists or if `GameLib.App` is still the active project.
- [ ] Ensure `GameLib.DAL` migrations and seed work cleanly via `grid-runner` before debugging UI issues.
- [ ] Confirm no business logic or database access is placed in Views or code-behinds; all logic must stay in `BL` Facades and `ViewModels`.

