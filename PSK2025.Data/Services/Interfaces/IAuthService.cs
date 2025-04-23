using PSK2025.Data.Requests.Auth;
using PSK2025.Data.Responses.Auth;


namespace PSK2025.Data.Services.Interfaces;

public interface IAuthService
{
    Task<UserLoginResponse> UserLoginAsync(UserLoginRequest request, CancellationToken cancelationToken = default);
    Task<Result<GetRefreshTokenResponse>> GetRefreshTokenAsync(GetRefreshTokenRequest request, CancellationToken cancellationToken = default);
}
