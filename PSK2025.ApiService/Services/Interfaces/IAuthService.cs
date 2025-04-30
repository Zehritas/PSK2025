using PSK2025.Data;
using PSK2025.Data.Requests.Auth;
using PSK2025.Data.Responses.Auth;
using PSK2025.Models.Entities;

namespace PSK2025.ApiService.Services.Interfaces;

public interface IAuthService
{
    Task<UserLoginResponse> UserLoginAsync(UserLoginRequest request, CancellationToken cancelationToken = default);
    Task<Result<GetRefreshTokenResponse>> GetRefreshTokenAsync(GetRefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result<User>> RegisterNewUserAsync(RegisterNewUserRequest request, CancellationToken cancellationToken = default);
}
