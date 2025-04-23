using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Settings;

public class TokenSettings
{
    public JwtSettings Jwt { get; init; } = new();
    public RefreshTokenSettings RefreshToken { get; init; } = new();
}

public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenExpirationMinutes { get; set; } = 60;
}

public class RefreshTokenSettings
{
    public int RefreshTokenExpirationDays { get; set; } = 7;
}
