using Microsoft.EntityFrameworkCore;
using PSK2025.Data.Contexts;
using PSK2025.Data.Repositories.Interfaces;
using PSK2025.Models.Entities;

namespace PSK2025.Data.Repositories
{
    public class UserProjectRepository : IUserProjectRepository
    {
        private readonly AppDbContext _context;

        public UserProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExistsAsync(String userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        public async Task<bool> ProjectExistsAsync(Guid projectId)
        {
            return await _context.Projects.AnyAsync(p => p.Id == projectId);
        }

        public async Task<bool> IsUserAssignedToProjectAsync(String userId, Guid projectId)
        {
            return await _context.UserProjects
                .AnyAsync(up => up.UserId == userId && up.ProjectId == projectId);
        }

        public async Task AssignUserToProjectAsync(UserProject userProject)
        {
            _context.UserProjects.Add(userProject);
            await _context.SaveChangesAsync();
        }

        public async Task<UserProject?> GetAssignmentAsync(String userId, Guid projectId)
        {
            return await _context.UserProjects.FindAsync(userId, projectId);
        }

        public async Task RemoveAssignmentAsync(UserProject userProject)
        {
            _context.UserProjects.Remove(userProject);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsersByProjectIdAsync(Guid projectId)
        {
            return await _context.UserProjects
                .Where(up => up.ProjectId == projectId)
                .Select(up => up.User)
                .ToListAsync();
        }
    }
}