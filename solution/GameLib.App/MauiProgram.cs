using CommunityToolkit.Maui;
using GameLib.App.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
//using Microsoft.Maui.DevFlow.Agent;
using System.Reflection;
using GameLib.DAL;
using GameLib.DAL.Migrator;
using GameLib.DAL.Seeds;
using GameLib.DAL.Options;
using CookBook.BL;


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

        ConfigureAppSettings(builder);


        builder.Services
            .AddDALServices()
            .AddBLServices()
            .AddAppServices();

//#if DEBUG
//        builder.AddMauiDevFlowAgent();
//#endif

        var app = builder.Build();

        AssertDALOptionsConfiguration(app);
        MigrateDb(app.Services.GetRequiredService<IDbMigrator>());
        SeedDb(app.Services.GetRequiredService<IDbSeeder>());
        RegisterRouting(app.Services.GetRequiredService<INavigationService>());

        return app;
    }
    

    private static void ConfigureAppSettings(MauiAppBuilder builder) {
        var configurationBuilder = new ConfigurationBuilder();

        var assembly = Assembly.GetExecutingAssembly();
        const string appSettingsFilePath = "GameLib.App.appsettings.json";
        using var appSettingsStream = assembly.GetManifestResourceStream(appSettingsFilePath);
        if (appSettingsStream is not null)
        {
            configurationBuilder.AddJsonStream(appSettingsStream);
        }

        var configuration = configurationBuilder.Build();
        builder.Configuration.AddConfiguration(configuration);

        builder.Services.Configure<DALOptions>(builder.Configuration.GetSection("GameLib:DAL"));
        builder.Services.PostConfigure<DALOptions>(options =>
        {
            if (string.IsNullOrWhiteSpace(options.DatabaseDirectory))
            {
                options.DatabaseDirectory = FileSystem.AppDataDirectory;
            }
        });
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

    private static void AssertDALOptionsConfiguration(MauiApp app)
    {
        var dalOptions = app.Services.GetRequiredService<IOptions<DALOptions>>();

        if (dalOptions?.Value is null)
        {
            throw new InvalidOperationException("No persistence provider configured");
        }

        if (string.IsNullOrEmpty(dalOptions?.Value.DatabaseName))
        {
            throw new InvalidOperationException($"{nameof(DALOptions.DatabaseName)} is not set");
        }
    }
}