using Microsoft.AspNetCore.Mvc;
using PSK2025.ApiService.Interfaces;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Models.DTOs;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Project;

public class GetAllAsyncEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Project;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/all", async ( IProjectService service) =>
            {
                var result = await service.GetAllAsync();
                return Results.Ok(result.Select(r => r.Project));
            })
            .WithName("Get All Projects")
            .Produces<IEnumerable<ProjectDto>>(200)
            .Produces(500);
    }
}