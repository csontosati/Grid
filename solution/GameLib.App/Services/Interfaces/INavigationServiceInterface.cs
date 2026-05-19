using GameLib.App.Models;

namespace GameLib.App.Services;

public interface INavigationService
{
    IEnumerable<RouteModel> Routes { get; }

    Task GoToAsync(string route);

    Task GoToDataAsync(string route, IDictionary<string, object?> parameters);
}

