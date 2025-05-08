using PSK2025.ApiService.Interfaces;
using PSK2025.Data.Requests.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Models.DTOs;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Project;

public class GetByIdAsyncEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Project;
    
    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/{id}", async ([FromRoute] Guid id, IProjectService service) =>
            {
                var request = new ProjectRequest(id);
                var user = await service.GetByIdAsync(request);

                return user is not null
                    ? Results.Ok(user)
                    : Results.NotFound();
            })
            .WithName("Get User")
            .Produces<UserDto>(200)
            .Produces(404)
            .Produces(500);
    }
    
}