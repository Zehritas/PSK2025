using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data;
using PSK2025.Data.Requests.Auth;
using PSK2025.Data.Responses.Auth;
using PSK2025.Models.DTOs;

namespace PSK2025.ApiService.Services.Decorators;

// This decorator implements validation logic for IAuthService.
// It intercepts calls before reaching the actual AuthService implementation,
// allowing us to validate input without modifying the core service code.
// This is an example of Glass-box extensibility — we can extend behavior
// (like validation) externally while still having visibility into the structure
// of the system.

public class AuthServiceValidationDecorator : IAuthService
{
    private readonly IAuthService _authService;
    private readonly IValidationService _validationService;

    public AuthServiceValidationDecorator(IAuthService authService, IValidationService validationService)
    {
        _authService = authService;
        _validationService = validationService;
    }

    public async Task<Result<UserLoginResponse>> UserLoginAsync(UserLoginRequest request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"DECORATOR: Processing login for {request.Email}");
        var validationResult = await _validationService.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<UserLoginResponse>.Failure(Error.ValidationError(validationResult.Errors));
        }

        // Delegate to original service (which also has its own validation)
        var result = await _authService.UserLoginAsync(request, cancellationToken);
        return result;
    }

    public async Task<Result<GetRefreshTokenResponse>> GetRefreshTokenAsync(GetRefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        // Simply delegate to original service
        return await _authService.GetRefreshTokenAsync(request, cancellationToken);
    }

    public async Task<Result<UserDto>> RegisterNewUserAsync(RegisterNewUserRequest request, CancellationToken cancellationToken = default)
    {
        // Simply delegate to original service
        return await _authService.RegisterNewUserAsync(request, cancellationToken);
    }
}