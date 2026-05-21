using System.Collections.Generic;
using System.Threading.Tasks;
using GameLib.App.Models;
using GameLib.App.Services.Interfaces;
using GameLib.App.Views;

namespace GameLib.App.Services;

public class NavigationService : INavigationService
{
    public const string LandingPageRouteAbsolute = "//UserSelectionView";
    public const string UserAddPageRouteRelative = "/SignUpView";
    public const string LibraryPageRouteAbsolute = "//DiscoverView";
    public const string GameAddPageRouteAbsolute = "//GameAddView";
    public const string UserSettingsPageAbsolute = "//UserSettings";


    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new(LandingPageRouteAbsolute, typeof(UserSelectionView)),
        new(LandingPageRouteAbsolute + UserAddPageRouteRelative, typeof(SignUpView)),
        new(LibraryPageRouteAbsolute, typeof(LibraryView)),
        new(GameAddPageRouteAbsolute, typeof(GameAddView)),
        new(UserSettingsPageAbsolute, typeof(UserSettingsView))
    };

    public Task GoToAsync(string route)
        => Shell.Current.GoToAsync(route);

    public Task GoToDataAsync(string route, IDictionary<string, object?> parameters)
        => Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed() => Shell.Current.SendBackButtonPressed();
}