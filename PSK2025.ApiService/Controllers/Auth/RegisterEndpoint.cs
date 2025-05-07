using PSK2025.ApiService.Interfaces;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Models.Enums;
using PSK2025.Data.Requests.Auth;
using Microsoft.AspNetCore.Mvc;
using PSK2025.Models.DTOs;
using PSK2025.ApiService.Extensions;

namespace PSK2025.ApiService.Controllers.Auth;

public class RegisterEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Auth;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapPost("/register", async ([FromBody] RegisterNewUserRequest request, IAuthService service) =>
        {
            var result = await service.RegisterNewUserAsync(request);

            return result.IsSuccess ?
                Results.Ok(new { Message = "User registered successfully." }) :
                result.Error.MapErrorToResponse();
        })
            .WithName("Register")
            .Produces(200)
            .Produces(400);
    }
}