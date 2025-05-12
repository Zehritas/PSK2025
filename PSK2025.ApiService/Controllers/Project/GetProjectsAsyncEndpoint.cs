using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSK2025.ApiService.Interfaces;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Models.DTOs;
using PSK2025.Models.Entities;
using PSK2025.Models.Enums;


namespace PSK2025.ApiService.Controllers.Project;

public class GetProjectsAsyncEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Project;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/", async (
                [FromQuery] int pageNumber,
                [FromQuery] int pageSize,
                [FromQuery] ProjectStatus? status,
                IProjectService service) =>
            {
                var result = await service.GetProjectsAsync(pageNumber, pageSize, status);
                
                return Results.Ok(new PaginatedResult<ProjectDto>
                {
                    Items = result.Items.Select(r => r.Project).ToList(),
                    TotalCount = result.TotalCount,
                    CurrentPage = result.CurrentPage,
                    PageSize = result.PageSize
                });
            })
            .RequireAuthorization()
            .WithName("Get All Projects")
            .Produces<PaginatedResult<ProjectDto>>(200)
            .Produces(500);
    }
}