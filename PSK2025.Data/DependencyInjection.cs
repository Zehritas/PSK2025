using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PSK2025.Data.Services.Interfaces;
using PSK2025.Data.Services;
using PSK2025.Data.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenSettings>(configuration);
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
