using GameLib.App.ViewModels;
using GameLib.App.Views;
using Microsoft.Extensions.DependencyInjection;
using ServiceScan.SourceGenerator;

namespace GameLib.App.Extensions;

public static partial class ServiceCollectionExtensions
{
    [GenerateServiceRegistrations(AssignableTo = typeof(ContentPageBase), AsSelf = true, Lifetime = ServiceLifetime.Transient)]
    public static partial IServiceCollection AddViews(this IServiceCollection services);

    [GenerateServiceRegistrations(AssignableTo = typeof(ViewModelBase), AsSelf = true, Lifetime = ServiceLifetime.Transient)]
    public static partial IServiceCollection AddViewModels(this IServiceCollection services);
}