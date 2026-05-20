using GameLib.App.Models;

namespace GameLib.App.Services.Interfaces;

public interface INavigationService
{
    IEnumerable<RouteModel> Routes { get; }

    Task GoToAsync(string route);

    Task GoToAsync(string route, IDictionary<string, object?> parameters);
}

