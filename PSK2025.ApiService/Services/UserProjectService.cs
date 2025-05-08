using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Contexts;
using PSK2025.Data.Requests.UserProject;
using PSK2025.Data.Responses.UserProject;
using PSK2025.Models.DTOs;
using PSK2025.Models.Entities;

namespace PSK2025.ApiService.Services;

public class UserProjectService : IUserProjectService
{
    private readonly AppDbContext _context;

    public UserProjectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProjectResponse> AssignAsync(AssignUserToProjectRequest request)
    {
        var entity = new UserProject
        {
            UserId = request.UserProject.UserId,
            ProjectId = request.UserProject.ProjectId
        };

        _context.UserProjects.Add(entity);
        await _context.SaveChangesAsync();

        return new UserProjectResponse(new UserProjectDto
        {
            UserId = entity.UserId,
            ProjectId = entity.ProjectId
        });
    }
}
