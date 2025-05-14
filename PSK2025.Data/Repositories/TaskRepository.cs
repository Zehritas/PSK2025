using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSK2025.Data.Abstractions;
using PSK2025.Models.Entities;
using PSK2025.Data.Contexts;
using PSK2025.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskEntity = PSK2025.Models.Entities.Task;

namespace PSK2025.Data.Repositories;

public class TaskRepository(AppDbContext dbContext) : GenericRepository<TaskEntity>(dbContext), ITaskRepository
{
    public async Task<List<TaskEntity>> GetListAsync(
        Guid? projectId = null,
        string? userId = null,
        int skip = 0,
        int take = 50,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Tasks.AsQueryable();

        if (projectId.HasValue)
        {
            query = query.Where(t => t.Projectid == projectId.Value);
        }

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(t => t.UserId == userId);
        }

        return await query
            .OrderBy(t => t.StartedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TaskEntity>> GetUserAccessibleTasksAsync(
        string currentUserId,
        Guid? projectId = null,
        string? userId = null,
        int skip = 0,
        int take = 50,
        CancellationToken cancellationToken = default)
    {
        var userProjectIds = await Context.UserProjects
            .Where(up => up.UserId == currentUserId)
            .Select(up => up.ProjectId)
            .ToListAsync(cancellationToken);
        
        var query = Context.Tasks.AsQueryable()
            .Where(t => userProjectIds.Contains(t.Projectid) || t.UserId == currentUserId);
        
        if (projectId.HasValue)
        {
            query = query.Where(t => t.Projectid == projectId.Value);
        }

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(t => t.UserId == userId);
        }

        return await query
            .OrderBy(t => t.StartedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsUserInProjectAsync(Guid projectId, string userId,
        CancellationToken cancellationToken = default)
    {
        return await Context.UserProjects
            .AnyAsync(up => up.ProjectId == projectId && up.UserId == userId, cancellationToken);
    }

    public override async Task<TaskEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Tasks
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }


}
