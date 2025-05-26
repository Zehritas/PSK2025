using Microsoft.AspNetCore.Authorization;
using PSK2025.ApiService.Interfaces;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Models.Entities;
using PSK2025.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using PSK2025.Data.Requests;
using PSK2025.Data.Requests.Task;
using PSK2025.ApiService.Extensions;
using PSK2025.Models.DTOs;

namespace PSK2025.ApiService.Controllers.Task;

public class GetTasksEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Task;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/",
                async ([FromQuery] int pageNumber,
                    [FromQuery] int pageSize,
                    [FromQuery] Guid? projectId,
                    [FromQuery] String? userId,
                    [FromQuery] PriorityStatus? priority,
                    [FromQuery] TaskEntityStatus? status,

                    ITaskService service) =>
                {
                    var request = new GetTasksRequest(
                        projectId,
                        userId,
                        priority,
                        status,
                        new GetPagedListRequest(pageNumber, pageSize)
                    );

                    var result = await service.GetTasksAsync(request);
    
                    return Results.Ok(new PaginatedResult<TaskDto>
                    {
                        Items = result.Items.ToList(),
                        TotalCount = result.TotalCount,
                        CurrentPage = result.CurrentPage,
                        PageSize = result.PageSize
                    });
                })
            .WithName("Get Tasks")
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .RequireAuthorization();
    }
}