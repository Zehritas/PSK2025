using PSK2025.ApiService.Interfaces;
using System.Reflection;

namespace PSK2025.ApiService.Extensions;

public static class ServiceRegistrationExtensions
{
    public static void AddEndpoints(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var endpointTypes = assembly.GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .ToList();

        foreach (var type in endpointTypes)
        {
            services.AddTransient(typeof(IEndpoint), type);
        }

        var routeGroupTypes = assembly.GetTypes()
            .Where(t => typeof(IRouteGroup).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .ToList();

        foreach (var type in routeGroupTypes)
        {
            services.AddTransient(typeof(IRouteGroup), type);
        }
    }
}