using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data;
using PSK2025.Data.Contexts;
using PSK2025.Data.Errors;
using PSK2025.Data.Requests.Auth;
using PSK2025.Data.Responses.Auth;
using PSK2025.Models.DTOs;
using PSK2025.Models.Entities;
using System.Net;

namespace PSK2025.ApiService.Services;

public class AuthService(
    UserManager<User> userManager,
    IJwtTokenService tokenService,
    AppDbContext context,
    IValidationService validationService,
    RoleManager<IdentityRole> roleManager) : IAuthService
{

    public async Task<Result<UserLoginResponse>> UserLoginAsync(UserLoginRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<UserLoginResponse>.Failure(Error.ValidationError(validationResult.Errors));
        }

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result<UserLoginResponse>.Failure(AuthErrors.UserInvalidCredentialsError);
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = tokenService.GenerateJwtToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken(user.Id);

        await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var response = new UserLoginResponse(token, user.Id, refreshToken.Token);
        return Result<UserLoginResponse>.Success(response);
    }

    public async Task<Result<GetRefreshTokenResponse>> GetRefreshTokenAsync(
    GetRefreshTokenRequest request,
    CancellationToken cancellationToken = default)
    {
        var validationResult = await validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<GetRefreshTokenResponse>.Failure(Error.ValidationError(validationResult.Errors));
        }

        var refreshToken = await context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt =>
                rt.Token == request.Token &&
                !rt.IsRevoked &&
                rt.ExpiresAt > DateTime.UtcNow,
                cancellationToken);

        if (refreshToken == null)
        {
            return Result<GetRefreshTokenResponse>.Failure(AuthErrors.InvalidTokenError);
        }

        refreshToken.Revoke();
        context.RefreshTokens.Update(refreshToken);
        await context.SaveChangesAsync(cancellationToken);

        var newAccessToken = tokenService.GenerateJwtToken(
            refreshToken.User,
            await userManager.GetRolesAsync(refreshToken.User));

        var newRefreshToken = tokenService.GenerateRefreshToken(refreshToken.User.Id);

        await context.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<GetRefreshTokenResponse>.Success(
            new GetRefreshTokenResponse(newAccessToken, newRefreshToken.Token));
    }

    public async Task<Result<UserDto>> RegisterNewUserAsync(RegisterNewUserRequest request,CancellationToken cancellationToken = default)
    {
        var validationResult = await validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<UserDto>.Failure(Error.ValidationError(validationResult.Errors));
        }

        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Result<UserDto>.Failure(AuthErrors.UserNotFound);
        }

        var user = new User
        {
            Email = request.Email,
            EmailConfirmed = true,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email
        };

        var createResult = await userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            return Result<UserDto>.Failure(AuthErrors.FailedToCreateUserError(createResult.Errors));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        await userManager.AddToRoleAsync(user, "User");

        await context.SaveChangesAsync(cancellationToken);

        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };

        return Result<UserDto>.Success(userDto);
    }

}
