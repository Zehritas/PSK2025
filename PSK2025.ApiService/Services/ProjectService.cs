using Microsoft.EntityFrameworkCore;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Contexts;
using PSK2025.Data.Requests.Project;
using PSK2025.Data.Responses.Project;
using PSK2025.Models.DTOs;
using PSK2025.Models.Entities;

namespace PSK2025.ApiService.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request)
    {
        var entity = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Project.Name,
            Status = request.Project.Status
        };

        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();

        return new ProjectResponse(new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Status = entity.Status
        });
    }

    public async Task<ProjectResponse> UpdateAsync(UpdateProjectRequest request)
    {
        var entity = await _context.Projects.FindAsync(request.Project.Id);
        if (entity == null) throw new KeyNotFoundException("Project not found");

        entity.Name = request.Project.Name;
        entity.Status = request.Project.Status;

        await _context.SaveChangesAsync();

        return new ProjectResponse(new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Status = entity.Status
        });
    }

    public async Task<ProjectResponse> GetByIdAsync(ProjectRequest request)
    {
        var entity = await _context.Projects.FindAsync(request.id);
        if (entity == null) throw new KeyNotFoundException("Project not found");

        return new ProjectResponse(new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Status = entity.Status
        });
    }

    public async Task<IEnumerable<ProjectResponse>> GetAllAsync()
    {
        return await _context.Projects
            .Select(p => new ProjectResponse(new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Status = p.Status
            }))
            .ToListAsync();
    }

    public async Task DeleteAsync(ProjectRequest request)
    {
        var entity = await _context.Projects.FindAsync(request.id);
        if (entity == null) throw new KeyNotFoundException("Project not found");

        _context.Projects.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
