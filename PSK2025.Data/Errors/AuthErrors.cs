using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Errors;

public static class AuthErrors
{
    public static readonly Error UserInvalidCredentialsError = new(
        "User.InvalidCredentials",
        "Email or password is incorrect.",
        HttpStatusCode.Unauthorized);
    public static readonly Error InvalidTokenError = new("Token.Invalid", "Invalid or missing refresh token.");
    public static readonly Error UserAlreadyExistsError = new("User.Exists", "User already exists", HttpStatusCode.Conflict);
    public static readonly Error RoleAlreadyExistsError = new("User.RoleExists", "Role already exists", HttpStatusCode.Conflict);
    public static readonly Error FailedToAddRoleToUserError = new("User.FailedToAddRole", "Failed to add role to user");
    public static Error FailedToCreateUserError(IEnumerable<IdentityError> errors)
    {
        var passwordErrors = errors
            .Where(e => e.Code.Contains("Password"))
            .Select(e => e.Description);

        if (passwordErrors.Any())
        {
            return new Error(
                "CreateUserFailed",
                "Password does not meet the required strength: " + string.Join(", ", passwordErrors),
                HttpStatusCode.UnprocessableEntity);
        }

        return new Error(
            "CreateUserFailed",
            string.Join(", ", errors.Select(e => e.Description)),
            HttpStatusCode.InternalServerError);
    }
    public static readonly Error UserNotFound = new("User.FailedToFind", "User not found");
}
