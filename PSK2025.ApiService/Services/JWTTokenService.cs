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
    private readonly IConfiguration _configuration;

    public JwtTokenService(
    IOptions<JwtSettings> jwtOptions,
    IOptions<RefreshTokenSettings> refreshOptions,
    ILogger<JwtTokenService> logger,
    IConfiguration configuration)
    {
        _jwtSettings = jwtOptions.Value;
        _refreshTokenSettings = refreshOptions.Value;
        _logger = logger;
        _configuration = configuration;
    }

    public string GenerateJwtToken(User user, IList<string> roles)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expiryMinutes = int.Parse(_configuration["Jwt:TokenValidityMins"]!);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
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
