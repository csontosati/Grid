using System.Collections.Generic;
using System.Threading.Tasks;
using GameLib.App.Models;
using GameLib.App.Views;

namespace GameLib.App.Services;

public class NavigationService : INavigationService
{
    public const string LandingPageRouteAbsolute = "//UserSelectionView";
    public const string LibraryPageRouteAbsolute = "//LibraryView";
    public const string UserAddRouteAbsolute = "SignUpView";

    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new(LandingPageRouteAbsolute, typeof(UserSelectionView)),
        new(LibraryPageRouteAbsolute, typeof(LibraryView)),
        new(UserAddRouteAbsolute, typeof(SignUpView))


    };

    public Task GoToAsync(string route)
        => Shell.Current.GoToAsync(route);

    public Task GoToDataAsync(string route, IDictionary<string, object?> parameters)
        => Shell.Current.GoToAsync(route, parameters);

}