using PSK2025.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<TaskEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TaskEntity item, CancellationToken cancellationToken = default);
    void Update(TaskEntity item);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<TaskEntity>> GetListAsync(Guid projectid, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
}
