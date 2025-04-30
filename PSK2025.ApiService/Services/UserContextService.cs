using PSK2025.ApiService.Services.Interfaces;
using System.Security.Claims;

namespace PSK2025.ApiService.Services;

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    public string GetCurrentUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user == null || (!user.Identity?.IsAuthenticated ?? true))
        {
            throw new UnauthorizedAccessException("The user is not authenticated.");
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new InvalidOperationException("The user ID claim is missing.");
        }

        return userId;
    }

    public string GetCurrentUsername()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user == null || (!user.Identity?.IsAuthenticated ?? true))
        {
            throw new UnauthorizedAccessException("The user is not authenticated.");
        }

        var username = user.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(username))
        {
            throw new InvalidOperationException("The username claim is missing.");
        }

        return username;
    }
}
