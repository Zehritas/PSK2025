using Microsoft.AspNetCore.Authorization;
using PSK2025.ApiService.Interfaces;
using PSK2025.Models.Entities;
using PSK2025.Models.Enums;
using PSK2025.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PSK2025.Data.Errors;
using PSK2025.ApiService.Extensions;

namespace PSK2025.ApiService.Controllers.Task;

public class DeleteTaskEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Task;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapDelete("/delete",
                async ([FromQuery] Guid id,
                    ITaskService service) =>
                {
                    var result = await service.DeleteTaskByIdAsync(id);

                    return result.IsSuccess
                        ? Results.Ok(new { Message = "Task deleted successfully." })
                        : result.Error.MapErrorToResponse();
                })
            //.RequireAuthorization(new AuthorizeAttribute
            //{
            //    Roles = $"{Roles.Admin.ToString()}, {Roles.User.ToString()}"
            //})
            .WithName("Delete Order")
            .Produces(200)
            .Produces(400)
            .Produces(500);
    }
}