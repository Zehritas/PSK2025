using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.ApiService.Settings;
using PSK2025.Models.Entities;

namespace PSK2025.ApiService.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly RefreshTokenSettings _refreshTokenSettings;
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(
        IOptions<TokenSettings> tokenSettingsOptions,
        ILogger<JwtTokenService> logger)
    {
        var tokenSettings = tokenSettingsOptions.Value;
        _jwtSettings = tokenSettings.Jwt;
        _refreshTokenSettings = tokenSettings.RefreshToken;
        _logger = logger;
    }


    public string GenerateJwtToken(User user, IList<string> roles)
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), 
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
            new(ClaimTypes.Name, user.UserName!)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        _logger.LogInformation("Generated JWT token for user {UserId}", user.Id);

        return jwt;
    }

    public RefreshToken GenerateRefreshToken(string userId)
    {
        var plainToken = GenerateSecureToken();
        var hashedToken = HashToken(plainToken);

        var refreshToken = RefreshToken.Create
        (
            hashedToken,
            DateTime.UtcNow.AddDays(_refreshTokenSettings.RefreshTokenExpirationDays),
            userId
        );

        _logger.LogInformation("Generated refresh token for user {UserId}", userId);

        return refreshToken;
    }



    private static string GenerateSecureToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public static string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}
