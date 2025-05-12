using Microsoft.AspNetCore.Mvc;
using PSK2025.ApiService.Interfaces;
using PSK2025.Models.DTOs;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Requests.Project;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Project;

public class GetProjectUsersEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Project;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/{id}/users", async ([FromRoute] Guid id, IProjectService service) =>
            {
                var users = await service.GetProjectUsersAsync(id);
                return Results.Ok(users);
            })
            .WithName("Get Project Users")
            .Produces<List<UserDto>>(200)
            .Produces(404);
    }
}