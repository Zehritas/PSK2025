using Microsoft.EntityFrameworkCore;
using PSK2025.Data.Contexts;
using PSK2025.Data.Repositories.Interfaces;
using PSK2025.Models.Entities;
namespace PSK2025.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects.ToListAsync();
            
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<Project> CreateAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }
        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}