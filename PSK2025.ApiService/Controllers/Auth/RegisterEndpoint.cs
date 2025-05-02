using PSK2025.ApiService.Interfaces;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Enums;
using PSK2025.Data.Requests.Auth;
using Microsoft.AspNetCore.Mvc;
using PSK2025.Models.DTOs;

namespace PSK2025.ApiService.Controllers.Auth;

public class RegisterEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Auth;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapPost("/register", async ([FromBody] RegisterNewUserRequest request, IAuthService service) =>
        {
            var result = await service.RegisterNewUserAsync(request);

            if (!result.IsSuccess)
                return Results.BadRequest(new { result.Error });

            return Results.Ok(result.Value);
        })
            .WithName("Register")
            .AllowAnonymous()
            .Produces<UserDto>(200)
            .Produces(400);
    }
}