using Microsoft.AspNetCore.Identity;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data;
using PSK2025.Data.Repositories.Interfaces;
using PSK2025.Data.Requests.User;
using PSK2025.Data.Responses;
using PSK2025.Models.Entities;
using PSK2025.MigrationService.Abstractions;
using PSK2025.Models.DTOs;

namespace PSK2025.ApiService.Services;

public class UserService(IUserRepository userRepository) : IUserService
{

    public async Task<UserDto?> GetUserByIdAsync(GetUserByIdAsyncRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            return null;

        return new UserDto()
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
        };
    }

}

