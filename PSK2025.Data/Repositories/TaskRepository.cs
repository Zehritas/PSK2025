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

public class TaskRepository(AppDbContext dbContext) : GenericRepository<Models.Entities.Task>(dbContext), ITaskRepository
{
    public async Task<List<TaskEntity>> GetListAsync(Guid projectId, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
    {
        return await Context.Tasks
            .Where(t => t.Projectid == projectId)
            .OrderBy(t => t.StartedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public override async Task<TaskEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Tasks
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }
}
