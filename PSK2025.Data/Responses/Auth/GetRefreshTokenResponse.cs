using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Responses.Auth;

public record GetRefreshTokenResponse(string AccessToken, string RefreshToken);