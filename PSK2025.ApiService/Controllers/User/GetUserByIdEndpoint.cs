using Microsoft.AspNetCore.Authorization;

namespace PSK2025.ApiService.Controllers.User;

//public class GetUserByIdEndpoint : IEndpoint
//{
//    public RouteGroupName Group => RouteGroupName.Users;

//    public void MapEndpoints(RouteGroupBuilder group)
//    {
//        group.MapGet("/{Id}",
//        async ([FromQuery] Guid id,
//            IUserService service) =>
//        {
//            var result = await service.GetUserByIdAsync(id);

//            return result.IsSuccess
//                ? Results.Ok(result.Value)
//                : result.Error.MapErrorToResponse();
//        })
//            .RequireAuthorization(new AuthorizeAttribute
//            {
//                Roles = $"{Roles.Admin.ToString()}, {Roles.User.ToString()}"
//            })
//            .WithName("Get user")
//            .Produces(200)
//            .Produces(400)
//            .Produces(500);
//    }
//}

