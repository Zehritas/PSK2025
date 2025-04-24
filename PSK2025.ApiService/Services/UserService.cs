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

public class UserService(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    UserManager<User> userManager) : IUserService
{
    // public async Task<Result<GetUserByIdResponse>> GetUserByIdAsync(Guid Id, CancellationToken cancellationToken = default)
    // {
    //     var currentUserId = userContextService.GetCurrentUserId();
    //     var currentUser = await userManager.FindByIdAsync(currentUserId);
    //
    //     if (currentUser is null)
    //         return Result.Failure<GetUserByIdResponse>(AuthErrors.UserNotFound);
    //
    //     var user = await userRepository.GetByIdAsync(Id, cancellationToken);
    //
    //     if (user == null)
    //         return Result.Failure<GetUserByIdResponse>(UserErrors.UserNotFoundError);
    //
    //     return Result<GetUserByIdResponse>.Success(
    //         new GetUserByIdResponse(
    //             new UserDto(
    //                 user.Id,
    //                 user.BusinessId
    //             )
    //         )
    //     );
    // }
    
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

