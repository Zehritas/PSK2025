using Microsoft.AspNetCore.Authorization;
using PSK2025.ApiService.Interfaces;
using PSK2025.Models.Entities;
using PSK2025.Models.Enums;
using PSK2025.Data.Requests.Task;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.ApiService.Extensions;


namespace PSK2025.ApiService.Controllers.Task;

public class CreateTaskEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Task;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapPost("/create",
                async (CreateTaskRequest request,
                    ITaskService service) =>
                {
                    var result = await service.CreateTaskAsync(request);

                    return result.IsSuccess
                        ? Results.Ok(new { Message = "Task created successfully.", Id = result.Value })
                        : result.Error.MapErrorToResponse();
                })
            //.RequireAuthorization(new AuthorizeAttribute
            //{
            //    Roles = $"{Roles.Admin.ToString()}, {Roles.Manager.ToString()}"
            //})
            .WithName("Create Discount")
            .Produces(200)
            .Produces(400)
            .Produces(500);
    }
}

