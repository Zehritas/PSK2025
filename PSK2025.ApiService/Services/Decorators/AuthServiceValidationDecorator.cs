using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data;
using PSK2025.Data.Requests.Auth;
using PSK2025.Data.Responses.Auth;
using PSK2025.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace PSK2025.ApiService.Services.Decorators;

public class AuthServiceValidationDecorator : IAuthService
{
    private readonly IAuthService _authService;
    private readonly IValidationService _validationService;
    private readonly ILogger<AuthServiceValidationDecorator> _logger;

    public AuthServiceValidationDecorator(
        IAuthService authService,
        IValidationService validationService,
        ILogger<AuthServiceValidationDecorator> logger)
    {
        _authService = authService;
        _validationService = validationService;
        _logger = logger;
    }

    public async Task<Result<UserLoginResponse>> UserLoginAsync(UserLoginRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing login for {Email}", request.Email);
        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for login request: {Errors}", string.Join(", ", validationResult.Errors));
            return Result<UserLoginResponse>.Failure(Error.ValidationError(validationResult.Errors));
        }

        var result = await _authService.UserLoginAsync(request, cancellationToken);
        return result;
    }

    public async Task<Result<GetRefreshTokenResponse>> GetRefreshTokenAsync(GetRefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing refresh token request for token {Token}", request.Token);
        return await _authService.GetRefreshTokenAsync(request, cancellationToken);
    }

    public async Task<Result<UserDto>> RegisterNewUserAsync(RegisterNewUserRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing user registration for {Email}", request.Email);
        return await _authService.RegisterNewUserAsync(request, cancellationToken);
    }
}
