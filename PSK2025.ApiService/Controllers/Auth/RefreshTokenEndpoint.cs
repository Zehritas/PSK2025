using PSK2025.ApiService.Interfaces;
using PSK2025.Data.Requests.Auth;
using PSK2025.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PSK2025.ApiService.Services.Interfaces;

namespace PSK2025.ApiService.Controllers.Auth;

public class RefreshTokenEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Auth;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapPost("/refresh-token", async ([FromBody] GetRefreshTokenRequest request, IAuthService service) =>
        {
            var result = await service.GetRefreshTokenAsync(request);

            if (!result.IsSuccess)
                return Results.Json(new { error = result.Error }, statusCode: StatusCodes.Status401Unauthorized);

            return Results.Ok(new
            {
                result.Value!.AccessToken,
                result.Value.RefreshToken
            });
        })
        .WithName("RefreshToken")
        .Produces(200)
        .Produces(401);
    }
}