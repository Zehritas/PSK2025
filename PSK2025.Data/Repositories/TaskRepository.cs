﻿using System;
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
using PSK2025.Models.Enums;

namespace PSK2025.Data.Repositories;

public class TaskRepository(AppDbContext dbContext) : GenericRepository<TaskEntity>(dbContext), ITaskRepository
{
    public async Task<List<TaskEntity>> GetUserAccessibleTasksAsync(
        string currentUserId,
        Guid? ProjectId = null,
        string? userId = null,
        PriorityStatus? priority = null,
        TaskEntityStatus? status = null,
        int skip = 0,
        int take = 50,
        CancellationToken cancellationToken = default)
    {

        var query = Context.Tasks
            .Include(t => t.User)
            .Where(t
            => t.Project.OwnerId == currentUserId || t.Project.UserProjects.Any(up => up.UserId == currentUserId));

        if (ProjectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == ProjectId.Value);
        }

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(t => t.UserId == userId);
        }

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        return await query
            .OrderBy(t => t.StartedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsUserInProjectAsync(Guid ProjectId, string userId,
        CancellationToken cancellationToken = default)
    {
        return await Context.UserProjects
            .AnyAsync(up => up.ProjectId == ProjectId && up.UserId == userId, cancellationToken);
    }

    public override async Task<TaskEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Tasks
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }


}
