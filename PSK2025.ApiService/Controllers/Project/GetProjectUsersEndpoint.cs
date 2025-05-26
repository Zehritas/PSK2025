using Microsoft.AspNetCore.Mvc;
using PSK2025.ApiService.Interfaces;
using PSK2025.Models.DTOs;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Requests.Project;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Project;

public class GetProjectUsersEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Project;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/{id}/users", async (
                 [FromQuery] int? pageNumber,
                 [FromQuery] int? pageSize,
                 [FromRoute] Guid id,
                 IProjectService service
             ) =>
             {
                 var result = await service.GetProjectUsersAsync(id, pageNumber, pageSize);

                 return Results.Ok(new PaginatedResult<UserDto>
                 {
                     Items = result.Items.ToList(),
                     TotalCount = result.TotalCount,
                     CurrentPage = result.CurrentPage,
                     PageSize = result.PageSize
                 });
                 
             })
             .RequireAuthorization()
             .WithName("Get Project Users")
             .Produces<PaginatedResult<UserDto>>(200)
             .Produces(404);
    }
}
