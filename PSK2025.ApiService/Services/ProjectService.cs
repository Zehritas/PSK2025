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
using Microsoft.Extensions.Logging;
using PSK2025.Data.Errors;
using PSK2025.Data;

namespace PSK2025.ApiService.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    private readonly IUserContextService _userContextService;
    private readonly ILogger _logger;
    public ProjectService(AppDbContext context, IUserContextService userContextService, ILogger<ProjectService> logger)
    {
        _context = context;
        _userContextService = userContextService;
        _logger = logger;
    }

    public async Task<ProjectsResponse> CreateAsync(CreateProjectRequest request)
    {
        var ownerid = _userContextService.GetCurrentUserId(); 
        var startDate = request.StartDate ?? DateTime.UtcNow;
        DateTime endDate = request.EndDate ?? startDate.AddDays(30);
        ProjectStatus status = request.Status ?? ProjectStatus.Planned;
        
        if (request.EndDate.HasValue && request.EndDate.Value < startDate)
        {
            throw new ArgumentException("End date must be after the start date.");
        }

        var entity = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            OwnerId = ownerid,
            Description = request.Description,
            StartDate = startDate,
            EndDate = endDate,
            Status = status
        };

        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(p => p.Owner).LoadAsync();

        return new ProjectsResponse(new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Owner = new OwnerDto
            {
                Id = entity.Owner.Id, 
                FirstName = entity.Owner.FirstName!,
                LastName = entity.Owner.LastName!
            },
            Description = request.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Status = entity.Status
        });
    }


    public async Task<ProjectsResponse> UpdateAsync(Guid id, UpdateProjectRequest request)
    {
        var entity = await _context.Projects.Include("Owner")
                                   .Where(p => p.Id == id)
                                   .FirstOrDefaultAsync();
        if (entity == null) throw new KeyNotFoundException("Project not found");

        _logger.LogInformation("Current Project Version before update: {Version}", entity.Version);

        entity.Name = request.Name;
        entity.Status = request.Status;
        entity.Description = request.Description;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;

        await _context.SaveChangesAsync();

        return new ProjectsResponse(new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Status = entity.Status,
            Owner = new OwnerDto
            {
                Id = entity.Owner.Id, 
                FirstName = entity.Owner.FirstName!,
                LastName = entity.Owner.LastName!
            },
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate
        });
    }

    public async Task<ProjectsResponse> GetByIdAsync(ProjectRequest request)
    {
        var currentUserId = _userContextService.GetCurrentUserId();

        var entity = await _context.Projects
            .Include(p => p.Owner) 
            .Include(p => p.UserProjects) 
            .ThenInclude(up => up.User) 
            .Where(p => p.Id == request.id) 
            .FirstOrDefaultAsync(); 

        if (entity == null)
        {
            throw new KeyNotFoundException("Project not found");
        }

        var isOwner = entity.OwnerId == currentUserId;
        var isMember = entity.UserProjects.Any(up => up.UserId == currentUserId);

        if (!isOwner && !isMember)
        {

            throw new KeyNotFoundException("Project not found"); 
        }

        return new ProjectsResponse(new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Status = entity.Status,
            Description = entity.Description,
            Owner = new OwnerDto
            {
                Id = entity.Owner.Id, 
                FirstName = entity.Owner.FirstName,
                LastName = entity.Owner.LastName
            },
            StartDate = entity.StartDate,
            EndDate = entity.EndDate
        });
    }

    public async Task<PaginatedResult<ProjectsResponse>> GetProjectsAsync(int pageNumber, int pageSize, ProjectStatus? status = null)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, 50);
        var userId = _userContextService.GetCurrentUserId();

        var query = _context.Projects
            .Include(p => p.Owner) 
            .Include(p => p.UserProjects)
            .Where(p => p.OwnerId == userId || p.UserProjects.Any(up => up.UserId == userId));

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProjectsResponse(new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Status = p.Status,
                Owner = new OwnerDto
                {
                    Id = p.Owner.Id,
                    FirstName = p.Owner.FirstName,
                    LastName = p.Owner.LastName
                },
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate
            }))
            .ToListAsync();

        return new PaginatedResult<ProjectsResponse>
        {
            Items = items,
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize
        };
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
