using System.Collections.Generic;
using System.Threading.Tasks;
using GameLib.App.Models;

namespace GameLib.App.Services;

public class NavigationService : INavigationService
{
    public const string LandingPageRouteAbsolute = "//landing";
    public const string LibraryPageRouteAbsolute = "//library";
    public const string UserAddRouteAbsolute = "//userAdd";

    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new(LandingPageRouteAbsolute, typeof(LandingPage)),
        new(LibraryPageRouteAbsolute, typeof(LibraryPage)),
        new(UserAddRouteAbsolute, typeof(UserAddPage))


    };

    public Task GoToAsync(string route)
        => Shell.Current.GoToAsync(route);

    public Task GoToDataAsync(string route, IDictionary<string, object?> parameters)
        => Shell.Current.GoToAsync(route, parameters);

}