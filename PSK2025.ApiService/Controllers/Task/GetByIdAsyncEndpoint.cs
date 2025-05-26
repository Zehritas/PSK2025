using PSK2025.ApiService.Interfaces;
using PSK2025.Data.Requests.Task;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Models.DTOs;
using PSK2025.Models.Entities;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Task;

public class GetByIdAsyncEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Task;
    
    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/{id}", async ([FromRoute] Guid id,  ITaskService service) =>
            {
                var task = await service.GetByIdAsync(id);

                return task is not null
                    ? Results.Ok(task)
                    : Results.NotFound();
            })
            .RequireAuthorization()
            .WithName("Get Task")
            .Produces<TaskDto>(200)
            .Produces(404)
            .Produces(500);
    }
    
}