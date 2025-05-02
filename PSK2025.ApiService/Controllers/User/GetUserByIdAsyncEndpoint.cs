using PSK2025.ApiService.Interfaces;
using PSK2025.Data.Requests.User;
using PSK2025.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Models.DTOs;

namespace PSK2025.ApiService.Controllers.User;

public class GetUserById : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.User;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/{id}", async ([FromRoute] Guid id, IUserService service) =>
        {
            var request = new GetUserByIdAsyncRequest(id);
            var user = await service.GetUserByIdAsync(request);

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