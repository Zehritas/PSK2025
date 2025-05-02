using PSK2025.Models.Entities;

namespace PSK2025.ApiService.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateJwtToken(User user, IList<string> roles);

    RefreshToken GenerateRefreshToken(string userId);
}
