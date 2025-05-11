using PSK2025.Data.Requests.Project;
using PSK2025.Data.Responses.Project;
using PSK2025.Models.DTOs;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Services.Interfaces;

public interface IProjectService
{
    Task<ProjectsResponse> CreateAsync(CreateProjectRequest request);
    Task<ProjectsResponse> UpdateAsync(UpdateProjectRequest request);
    Task<ProjectsResponse> GetByIdAsync(ProjectRequest request);
    Task<PaginatedResult<ProjectsResponse>> GetProjectsAsync(int pageNumber, int pageSize, ProjectStatus? status = null);
    Task DeleteAsync(ProjectRequest request);
    
    Task<List<UserDto>> GetProjectUsersAsync(Guid projectId);
}