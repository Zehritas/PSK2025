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

namespace PSK2025.Data.Repositories;

public class TaskRepository(AppDbContext dbContext) : GenericRepository<TaskEntity>(dbContext), ITaskRepository
{
    //public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    //{
    //    return await dbContext.Users
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync(u => u.Id == id.ToString(), cancellationToken);
    //}

    //public async Task<List<TaskEntity>> GetListAsync(Guid businessId, int skip = 0, int take = 50,
    //    CancellationToken cancellationToken = default)
    //{
    //    return await Context.Tasks
    //        .Include(i => i.Variations)
    //        .Include(i => i.Business)
    //        .Where(i => i.Business.Id == businessId)
    //        .OrderBy(i => i.Id)
    //        .Skip(skip)
    //        .Take(take)
    //        .ToListAsync(cancellationToken);
    //}
}
