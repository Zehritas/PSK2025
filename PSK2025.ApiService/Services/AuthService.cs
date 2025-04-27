using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data;
using PSK2025.Data.Contexts;
using PSK2025.Data.Requests.Auth;
using PSK2025.Data.Responses.Auth;
using PSK2025.Models.Entities;

namespace PSK2025.ApiService.Services;

public class AuthService(
    UserManager<User> userManager,
    IConfiguration configuration,
    IJwtTokenService tokenService,
    AppDbContext context) : IAuthService
{

    public async Task<UserLoginResponse> UserLoginAsync(UserLoginRequest request, CancellationToken cancelationToken = default)
    {
        var user = await userManager.FindByNameAsync(request.Username);

        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = tokenService.GenerateJwtToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken(user.Id);

        context.RefreshTokens.Add(refreshToken); 
        await context.SaveChangesAsync(cancelationToken);

        return new UserLoginResponse(token, user.Id, refreshToken.Token);
    }

    public async Task<Result<GetRefreshTokenResponse>> GetRefreshTokenAsync(GetRefreshTokenRequest request,
    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            return Result<GetRefreshTokenResponse>.Failure("Refresh token must be provided.");
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
            return Result<GetRefreshTokenResponse>.Failure("Invalid or expired refresh token.");
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
    public async Task RegisterNewUserAsync(RegisterNewUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Failure(Error.ValidationError(validationResult.Errors));
        }

        var callingUserId = userContextService.GetCurrentUserId();
        var callingUser = await userManager.FindByIdAsync(callingUserId);

        if (callingUser is null)
        {
            return Result.Failure(AuthErrors.UserNotFound);
        }
        var businessId = callingUser.BusinessId;

        var existingUser = await userManager.FindByNameAsync(request.Username);
        if (existingUser != null)
        {
            return Result.Failure(AuthErrors.UserAlreadyExistsError);
        }

        var user = new User
        {
            UserName = request.Username,
            Email = request.Email,
            EmailConfirmed = true,
        };
        user.AssignBusiness(businessId);

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            return Result.Failure(AuthErrors.FailedToCreateUserError(createResult.Errors));
        }

        if (!await roleManager.RoleExistsAsync(request.Role))
        {
            return Result.Failure(AuthErrors.RoleAlreadyExistsError);
        }

        var addToRoleResult = await userManager.AddToRoleAsync(user, request.Role);

        if (!addToRoleResult.Succeeded)
        {
            return Result.Failure(AuthErrors.FailedToAddRoleToUserError);
        }

        return Result.Success();
    }
}
