using PSK2025.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskEntity = PSK2025.Models.Entities.Task;
using SystemTask = System.Threading.Tasks.Task;
using PSK2025.Models.Enums;

namespace PSK2025.Data.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<TaskEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    SystemTask AddAsync(TaskEntity item, CancellationToken cancellationToken = default);
    void Update(TaskEntity item);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<TaskEntity>> GetUserAccessibleTasksAsync(string currentUserId, Guid? projectId = null, string? userId = null, PriorityStatus? priority = null, TaskEntityStatus? status = null, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task<bool> IsUserInProjectAsync(Guid projectId, string userId, CancellationToken cancellationToken = default);
}
