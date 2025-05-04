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

    public string GetUserEmail()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user == null || (!user.Identity?.IsAuthenticated ?? true))
        {
            throw new UnauthorizedAccessException("The user is not authenticated.");
        }

        var email = user.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            throw new InvalidOperationException("The email claim is missing.");
        }

        return email;
    }
}
