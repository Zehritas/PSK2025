using Microsoft.AspNetCore.Authorization;
using PSK2025.ApiService.Interfaces;
using PSK2025.Models.Entities;
using PSK2025.Models.Enums;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Requests.Task;
using PSK2025.ApiService.Extensions;

namespace PSK2025.ApiService.Controllers.Task;

public class UpdateTaskEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Task;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapPut("/{id}",
                async (UpdateTaskRequest request,
                    ITaskService service) =>
                {
                    var result = await service.EditTaskAsync(request);

                    return result.IsSuccess
                        ? Results.Ok(new { Message = "Task updated successfully." })
                        : result.Error.MapErrorToResponse();
                })
            .WithName("Updated Task")
            .Produces(200)
            .Produces(400)
            .Produces(500);
    }
}
