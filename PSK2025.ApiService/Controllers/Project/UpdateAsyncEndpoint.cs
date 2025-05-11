using Microsoft.AspNetCore.Mvc;
using PSK2025.ApiService.Interfaces;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Requests.Project;
using PSK2025.Models.DTOs;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Project;

public class UpdateAsyncEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Project;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapPut("/", async ([FromBody] UpdateProjectRequest request, IProjectService service) =>
            {
                var result = await service.UpdateAsync(request);

                return Results.Ok(result.Project);
            })
            .WithName("Update Project")
            .Produces<ProjectDto>(200)
            .Produces(400)
            .Produces(404)
            .Produces(500);
    }
}