using Microsoft.AspNetCore.Authorization;
using PSK2025.ApiService.Interfaces;
using PSK2025.Models.Entities;
namespace PSK2025.ApiService.Extensions;

public static class EndpointExtensions
{
    public static RouteGroupBuilder RequireAuthorization(this RouteGroupBuilder builder, IEnumerable<Roles> roles)
    {
        return builder.RequireAuthorization(new AuthorizeAttribute { Roles = string.Join(",", roles.Select(r => r.ToString())) });
    }
    public static RouteHandlerBuilder RequireAuthorization(this RouteHandlerBuilder builder, IEnumerable<Roles> roles)
    {
        return builder.RequireAuthorization(new AuthorizeAttribute { Roles = string.Join(",", roles.Select(r => r.ToString())) });
    }

    public static void MapGroupedEndpoints(this WebApplication app)
    {
        var routeGroups = app.Services.GetServices<IRouteGroup>();
        var endpoints = app.Services.GetServices<IEndpoint>();

        var groupedEndpoints = endpoints.GroupBy(e => e.Group);

        foreach (var group in groupedEndpoints)
        {
            var routeGroup = routeGroups.FirstOrDefault(rg => rg.Group == group.Key);

            if (routeGroup == null)
            {
                throw new InvalidOperationException($"No route group found for {group.Key}");
            }

            var groupBuilder = app.MapGroup(routeGroup.RoutePrefix)
                .WithTags(routeGroup.Tag);

            foreach (var endpoint in group)
            {
                endpoint.MapEndpoints(groupBuilder);
            }
        }
    }
}
