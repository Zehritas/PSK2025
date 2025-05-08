using PSK2025.Models.Entities;

namespace PSK2025.Data.Repositories.Interfaces
{
    public interface IUserProjectRepository
    {
        Task<bool> UserExistsAsync(String userId);
        Task<bool> ProjectExistsAsync(Guid projectId);
        Task<bool> IsUserAssignedToProjectAsync(String userId, Guid projectId);
        Task AssignUserToProjectAsync(UserProject userProject);
        Task<UserProject?> GetAssignmentAsync(String userId, Guid projectId);
        Task RemoveAssignmentAsync(UserProject userProject);
        Task<List<User>> GetUsersByProjectIdAsync(Guid projectId);
    }
}