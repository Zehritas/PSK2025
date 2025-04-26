using PSK2025.ApiService.Interfaces;
using PSK2025.Data.Requests.User;
using PSK2025.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PSK2025.ApiService.Services.Interfaces;

namespace PSK2025.ApiService.Controllers.User;

public class GetUserById : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.User;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapPost("/get-user-by-id", async ([FromBody] GetUserByIdAsyncRequest request, IUserService service) =>
        {
            var user = await service.GetUserByIdAsync(request);

            if (user == null)
                return Results.NotFound();

            return Results.Ok(user);
        });
    }
}