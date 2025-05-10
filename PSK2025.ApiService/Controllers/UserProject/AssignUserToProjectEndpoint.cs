using Microsoft.AspNetCore.Mvc;
using PSK2025.ApiService.Interfaces;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Requests.UserProject;
using PSK2025.Data.Responses.UserProject;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.UserProject;

public class AssignUserToProjectEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.UserProject;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapPost("/assign-user", async ([FromBody] AssignUserToProjectRequest request, IUserProjectService service) =>
            {
                var response = await service.AssignAsync(request);
                return Results.Ok(response.UserProject);
            })
            .WithName("AssignUserToProject")
            .Produces<UserProjectResponse>(200)
            .Produces(400)
            .Produces(500);
    }
}