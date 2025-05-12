using PSK2025.Models.Entities;
using SystemTask = System.Threading.Tasks.Task;

namespace PSK2025.Data.Repositories.Interfaces
{
    public interface IUserProjectRepository
    {
        Task<bool> UserExistsAsync(String userId);
        Task<bool> ProjectExistsAsync(Guid projectId);
        Task<bool> IsUserAssignedToProjectAsync(String userId, Guid projectId);
        SystemTask AssignUserToProjectAsync(UserProject userProject);
        Task<UserProject?> GetAssignmentAsync(String userId, Guid projectId);
        SystemTask RemoveAssignmentAsync(UserProject userProject);
        Task<List<User>> GetUsersByProjectIdAsync(Guid projectId);
    }
}