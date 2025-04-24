using PSK2025.Data.Requests.User;
using PSK2025.Data.Responses;
using PSK2025.Models.DTOs;

namespace PSK2025.ApiService.Services.Interfaces;

public interface IUserService
{
    // Task<GetUserByIdResponse> GetUserByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<UserDto?> GetUserByIdAsync(GetUserByIdAsyncRequest request, CancellationToken cancellationToken = default);
}
