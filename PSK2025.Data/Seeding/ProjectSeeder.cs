using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using PSK2025.Models.Entities;
using PSK2025.Models.Enums;
using PSK2025.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PSK2025.Data.Seeding
{
    public class ProjectSeeder
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public ProjectSeeder(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async System.Threading.Tasks.Task SeedProjectsAsync()
        {
            // Ensure there is at least one user for the project owner
            var ownerId = await _userManager.Users.FirstOrDefaultAsync();
            if (ownerId == null)
            {
                throw new Exception("No users found to assign as project owners.");
            }

            // Seeding Projects
            if (!_context.Projects.Any())
            {
                var projects = new List<Project>
                {
                    new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = "Sample Project 1",
                        OwnerId = ownerId.Id, // Assign the first user as the owner
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddDays(30),
                        Status = ProjectStatus.Planned
                    },
                    new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = "Sample Project 2",
                        OwnerId = ownerId.Id, // Assign the first user as the owner
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddDays(60),
                        Status = ProjectStatus.Active
                    }
                };

                _context.Projects.AddRange(projects);
                await _context.SaveChangesAsync();
            }
        }
    }
}
