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
    public const string LibraryPageRouteRelative = "/LibraryView";
    public const string UserSettingsPageRouteRelative = "/UserSettingsView";

    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new(LandingPageRouteAbsolute, typeof(UserSelectionView)),
        new(LandingPageRouteAbsolute + UserAddPageRouteRelative, typeof(SignUpView)),
        new(LandingPageRouteAbsolute + LibraryPageRouteRelative, typeof(LibraryView)),
        new(LandingPageRouteAbsolute + LibraryPageRouteRelative + UserSettingsPageRouteRelative, typeof(UserSettingsView))
    };

    public Task GoToAsync(string route)
        => Shell.Current.GoToAsync(route);

    public Task GoToAsync(string route, IDictionary<string, object?> parameters)
        => Shell.Current.GoToAsync(route, parameters);

}