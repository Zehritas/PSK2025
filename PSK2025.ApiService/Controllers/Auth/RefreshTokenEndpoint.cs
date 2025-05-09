﻿using PSK2025.ApiService.Interfaces;
using PSK2025.Data.Requests.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Responses.Auth;
using PSK2025.ApiService.Extensions;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Auth;

public class RefreshTokenEndpoint : IEndpoint
{
    public RouteGroupName Group => RouteGroupName.Auth;

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapPost("/refresh-token", async ([FromBody] GetRefreshTokenRequest request, IAuthService service) =>
        {
            var result = await service.GetRefreshTokenAsync(request);

            return result.IsSuccess ?
                Results.Ok(new { result.Value!.AccessToken, result.Value.RefreshToken }) :
                result.Error.MapErrorToResponse();
        })
            .WithName("RefreshToken")
            .Produces(200)
            .Produces(401);

    }
}