using PSK2025.Data.Requests.UserProject;
using PSK2025.Data.Responses.UserProject;

namespace PSK2025.ApiService.Services.Interfaces;

public interface IUserProjectService
{
    Task<UserProjectResponse> AssignAsync(AssignUserToProjectRequest request);
    Task<UserProjectResponse> RemoveAsync(RemoveUserFromProjectRequest request);
}
