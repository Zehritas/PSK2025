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
    string GenerateJwtToken(User user, IList<string> roles);

    RefreshToken GenerateRefreshToken(string userId);
}
