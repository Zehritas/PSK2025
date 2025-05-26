using Microsoft.AspNetCore.Mvc;
using PSK2025.ApiService.Interfaces;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Requests.Project;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Project;

public class DeleteAsyncEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Project;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", async ([FromRoute] Guid id, IProjectService service) =>
            {
                var request = new ProjectRequest(id);
                await service.DeleteAsync(request);
                return Results.NoContent();
            })
             .RequireAuthorization()
            .WithName("Delete Project")
            .Produces(204)
            .Produces(404)
            .Produces(500);
    }
}