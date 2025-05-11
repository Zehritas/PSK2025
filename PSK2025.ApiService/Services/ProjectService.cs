using Microsoft.EntityFrameworkCore;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Contexts;
using PSK2025.Data.Requests.Project;
using PSK2025.Data.Responses.Project;
using PSK2025.Models.DTOs;
using PSK2025.Models.Entities;
using PSK2025.Models.Enums;
using PSK2025.Models.Entities;
using SystemTask = System.Threading.Tasks.Task;

namespace PSK2025.ApiService.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectsResponse> CreateAsync(CreateProjectRequest request)
    {
        // Validate that the OwnerId exists in the database
        var owner = await _context.Users.FindAsync(request.OwnerId);
        if (owner == null)
        {
            throw new KeyNotFoundException("Owner with the specified ID does not exist.");
        }

        // If StartDate is not provided, set it to the current date (now).
        var startDate = request.StartDate ?? DateTime.UtcNow;

        // If EndDate is not provided, set it to 30 days after the StartDate.
        DateTime endDate = request.EndDate ?? startDate.AddDays(30);

        // If EndDate is provided and it's before the StartDate, throw an error.
        if (request.EndDate.HasValue && request.EndDate.Value < startDate)
        {
            throw new ArgumentException("End date must be after the start date.");
        }

        var entity = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.ProjectName,
            OwnerId = request.OwnerId,
            StartDate = startDate,
            EndDate = endDate,
            Status = ProjectStatus.Planned
        };

        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();

        return new ProjectsResponse(new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            OwnerId = entity.OwnerId,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Status = entity.Status
        });
    }




    public async Task<ProjectsResponse> UpdateAsync(UpdateProjectRequest request)
    {
        var entity = await _context.Projects.FindAsync(request.Project.Id);
        if (entity == null) throw new KeyNotFoundException("Project not found");

        entity.Name = request.Project.Name;
        entity.Status = request.Project.Status;
        entity.OwnerId = request.Project.OwnerId;
        entity.StartDate = request.Project.StartDate;
        entity.EndDate = request.Project.EndDate;

        await _context.SaveChangesAsync();

        return new ProjectsResponse(new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Status = entity.Status,
            OwnerId = entity.OwnerId,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate
        });
    }

    public async Task<ProjectsResponse> GetByIdAsync(ProjectRequest request)
    {
        var entity = await _context.Projects.FindAsync(request.id);
        if (entity == null) throw new KeyNotFoundException("Project not found");

        return new ProjectsResponse(new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Status = entity.Status,
            OwnerId = entity.OwnerId,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate
        });
    }

    public async Task<IEnumerable<ProjectsResponse>> GetProjectsAsync(int pageNumber, int pageSize)
    {
        return await _context.Projects
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProjectsResponse(new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Status = p.Status,
                OwnerId = p.OwnerId,
                StartDate = p.StartDate,
                EndDate = p.EndDate
            }))
            .ToListAsync();
    }

    public async SystemTask DeleteAsync(ProjectRequest request)
    {
        var entity = await _context.Projects.FindAsync(request.id);
        if (entity == null) throw new KeyNotFoundException("Project not found");

        _context.Projects.Remove(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<UserDto>> GetProjectUsersAsync(Guid projectId)
    {
        var users = await _context.UserProjects
            .Where(up => up.ProjectId == projectId)
            .Select(up => up.User) // Get the User entities
            .ToListAsync();

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            // Map other fields if needed
        }).ToList();
    }

}
