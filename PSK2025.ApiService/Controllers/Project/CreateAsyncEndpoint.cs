using Microsoft.AspNetCore.Mvc;
using PSK2025.ApiService.Interfaces;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Requests.Project;
using PSK2025.Models.DTOs;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Project;

public class CreateAsyncEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Project;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapPost("/", async ([FromBody] CreateProjectRequest request, IProjectService service) =>
            {
                var result = await service.CreateAsync(request);
                return Results.Created($"/api/projects/{result.Project.Id}", result.Project);
            })
            .WithName("Create Project")
            .Produces<ProjectDto>(201)
            .Produces(400)
            .Produces(500);
    }
}