using PSK2025.ApiService.Interfaces;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Enums;
using PSK2025.Data.Requests.Auth;
using Microsoft.AspNetCore.Mvc;
using PSK2025.Data.Responses.Auth;
using PSK2025.Data;
using PSK2025.ApiService.Extensions;


namespace PSK2025.ApiService.Controllers.Auth;

public class LoginEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Auth;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapPost("/login", async ([FromBody] UserLoginRequest request, IAuthService service) =>
        {
            var result = await service.UserLoginAsync(request);

            return result.IsSuccess ?
                Results.Ok(result.Value) :
                result.Error.MapErrorToResponse();
        })
            .WithName("Login")
            .AllowAnonymous()
            .Produces(200)
            .Produces(401);
    }
}