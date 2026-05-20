using CommunityToolkit.Maui;
using GameLib.App.Services;
using GameLib.DAL;
using GameLib.DAL.Migrator;
using GameLib.DAL.Seeds;
using GameLib.BL;
using GameLib.App.Services.Interfaces;


namespace GameLib.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "GameLib.db");
        builder.Services
            .AddDALServices(dbPath)
            .AddBLServices()
            .AddAppServices();

        var app = builder.Build();

        MigrateDb(app.Services.GetRequiredService<IDbMigrator>());
        SeedDb(app.Services.GetRequiredService<IDbSeeder>());
        RegisterRouting(app.Services.GetRequiredService<INavigationService>());

        return app;
    }

    private static void RegisterRouting(INavigationService navigationService)
    {
        foreach (var route in navigationService.Routes)
        {
            Routing.RegisterRoute(route.Route, route.ViewType);
        }
    }

    private static void MigrateDb(IDbMigrator migrator) => migrator.Migrate();
    private static void SeedDb(IDbSeeder dbSeeder) => dbSeeder.Seed();
}