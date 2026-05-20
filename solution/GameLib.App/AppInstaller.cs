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

        

        services.AddViews();
        services.AddViewModels();
        services.AddSingleton<global::GameLib.App.ViewModels.UserListViewModel>();
        services.AddSingleton<global::GameLib.App.ViewModels.UserAddViewModel>();
        services.AddSingleton<global::GameLib.App.ViewModels.AppShellViewModel>();
        services.AddSingleton<global::GameLib.App.ViewModels.GameListViewModel>();
        services.AddSingleton<global::GameLib.App.ViewModels.GameAddViewModel>();
        services.AddSingleton<global::GameLib.App.ViewModels.UserSettingsViewModel>();
        services.AddSingleton<global::GameLib.App.ViewModels.GameDetailViewModel>();


        return services;
    }
}