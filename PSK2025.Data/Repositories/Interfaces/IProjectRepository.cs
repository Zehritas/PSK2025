using PSK2025.Models.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemTask = System.Threading.Tasks.Task;


namespace PSK2025.Data.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(Guid id);
        Task<Project> CreateAsync(Project project);
        SystemTask UpdateAsync(Project project);
        SystemTask DeleteAsync(Guid id);
    }
}