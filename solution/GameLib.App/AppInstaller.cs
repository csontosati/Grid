using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Extensions;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;

namespace GameLib.App;

public static class AppInstaller
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<AppShell>();

        services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);

        services.AddSingleton<IMessengerService, MessengerService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IAlertService, AlertService>();

        services.AddSingleton<AppState>();

        services.AddViews();
        services.AddViewModels();


        return services;
    }
}