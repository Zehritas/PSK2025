using PSK2025.Data.Requests.Project;
using PSK2025.Data.Responses.Project;

namespace PSK2025.ApiService.Services.Interfaces;

public interface IProjectService
{
    Task<ProjectResponse> CreateAsync(CreateProjectRequest request);
    Task<ProjectResponse> UpdateAsync(UpdateProjectRequest request);
    Task<ProjectResponse> GetByIdAsync(ProjectRequest request);
    Task<IEnumerable<ProjectResponse>> GetAllAsync();
    Task DeleteAsync(ProjectRequest request);
}