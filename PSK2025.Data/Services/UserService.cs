using Microsoft.AspNetCore.Identity;
using PSK2025.Data.DTOs;
using PSK2025.Data.Services.Interfaces;
using PSK2025.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Services;

//public class UserService(
//    IUnitOfWork unitOfWork,
//    IUserRepository userRepository,
//    UserManager<User> userManager,
//    IUserContextService userContextService) : IUserService
//{
//    public async Task<Result<GetUserByIdResponse>> GetUserByIdAsync(Guid Id, CancellationToken cancellationToken = default)
//    {
//        var currentUserId = userContextService.GetCurrentUserId();
//        var currentUser = await userManager.FindByIdAsync(currentUserId);

//        if (currentUser is null)
//            return Result.Failure<GetUserByIdResponse>(AuthErrors.UserNotFound);

//        var user = await userRepository.GetByIdAsync(Id, cancellationToken);

//        if (user == null)
//            return Result.Failure<GetUserByIdResponse>(UserErrors.UserNotFoundError);

//        return Result<GetUserByIdResponse>.Success(
//            new GetUserByIdResponse(
//                new UserDto(
//                    user.Id,
//                    user.BusinessId
//                )
//            )
//        );
//    }
//}

