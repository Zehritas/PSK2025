using PSK2025.Data.Requests.Project;
using PSK2025.Data.Responses.Project;
using PSK2025.Models.DTOs;

namespace PSK2025.ApiService.Services.Interfaces;

public interface IProjectService
{
    Task<ProjectsResponse> CreateAsync(CreateProjectRequest request);
    Task<ProjectsResponse> UpdateAsync(UpdateProjectRequest request);
    Task<ProjectsResponse> GetByIdAsync(ProjectRequest request);
    Task<IEnumerable<ProjectsResponse>> GetProjectsAsync(int pageNumber, int pageSize);
    Task DeleteAsync(ProjectRequest request);
    
    Task<List<UserDto>> GetProjectUsersAsync(Guid projectId);
}