using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Repositories.Interfaces;
using PSK2025.Data.Requests.UserProject;
using PSK2025.Data.Responses.UserProject;
using PSK2025.Models.DTOs;
using PSK2025.Models.Entities;

namespace PSK2025.ApiService.Services
{
    public class UserProjectService : IUserProjectService
    {
        private readonly IUserProjectRepository _repository;

        public UserProjectService(IUserProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserProjectResponse> AssignAsync(AssignUserToProjectRequest request)
        {
            var userId = request.UserProject.UserId;
            var projectId = request.UserProject.ProjectId;

            if (!await _repository.UserExistsAsync(userId))
                throw new InvalidOperationException($"User with ID {userId} does not exist.");

            if (!await _repository.ProjectExistsAsync(projectId))
                throw new InvalidOperationException($"Project with ID {projectId} does not exist.");

            if (await _repository.IsUserAssignedToProjectAsync(userId, projectId))
                throw new InvalidOperationException("User is already assigned to this project.");

            var entity = new UserProject { UserId = userId, ProjectId = projectId };
            await _repository.AssignUserToProjectAsync(entity);

            return new UserProjectResponse(new UserProjectDto
            {
                UserId = entity.UserId,
                ProjectId = entity.ProjectId
            });
        }

        public async Task<UserProjectResponse> RemoveAsync(RemoveUserFromProjectRequest request)
        {
            var entity = await _repository.GetAssignmentAsync(
                request.UserProject.UserId,
                request.UserProject.ProjectId
            );

            if (entity == null)
                throw new InvalidOperationException("User is not assigned to the project.");

            await _repository.RemoveAssignmentAsync(entity);

            return new UserProjectResponse(new UserProjectDto
            {
                UserId = entity.UserId,
                ProjectId = entity.ProjectId
            });
        }
        
    }
}
