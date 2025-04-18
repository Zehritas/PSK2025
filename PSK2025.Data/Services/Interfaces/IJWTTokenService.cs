using PSK2025.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Services.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateJwtTokenAsync(User user);
    Task<ClaimsPrincipal> ValidateTokenAsync(string token);
}
