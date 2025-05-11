using Microsoft.AspNetCore.Mvc;
using PSK2025.ApiService.Interfaces;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Requests.UserProject;
using PSK2025.Data.Responses.UserProject;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.UserProject;

public class RemoveUserFromProjectEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.UserProject;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapDelete("/", async ([FromBody] RemoveUserFromProjectRequest request, IUserProjectService service) =>
            {
                try
                {
                    var response = await service.RemoveAsync(request);
                    return Results.Ok(response.UserProject);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("RemoveUserFromProject")
            .Produces<UserProjectResponse>(200)
            .Produces(400)
            .Produces(500);
    }
}