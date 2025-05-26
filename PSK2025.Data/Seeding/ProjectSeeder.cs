using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PSK2025.Data.Contexts;
using PSK2025.Models.Entities;
using PSK2025.Models.Enums;
using TaskEntity = PSK2025.Models.Entities.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace PSK2025.Data.Seeding
{
    public class ProjectSeeder
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly Random _random = new();

        private static readonly string[] SampleTaskNames =
        {
            "Design database schema",
            "Implement API endpoints",
            "Write unit tests",
            "Create CI/CD pipeline",
            "Document endpoints",
            "Optimize queries",
            "Design UI mockups",
            "User authentication",
            "Role management",
            "Prepare demo presentation"
        };

        public ProjectSeeder(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async SystemTask SeedProjectsAsync()
        {
            if (_context.Projects.Any())
                return;

            var users = await _userManager.Users.ToListAsync();
            if (users.Count == 0)
                throw new Exception("No users found to assign as owners or assignees.");

            var projects = new List<Project>();
            var tasks = new List<TaskEntity>();

            int projectCount = 10;

            for (int i = 1; i <= projectCount; i++)
            {
                var owner = users[_random.Next(users.Count)];

                var project = new Project
                {
                    Id = Guid.NewGuid(),
                    Name = $"Project {i}",
                    OwnerId = owner.Id,
                    StartDate = DateTime.UtcNow.AddDays(-_random.Next(0, 180)),
                    EndDate = DateTime.UtcNow.AddDays(_random.Next(10, 120)),
                    Status = (Models.Enums.ProjectStatus)_random.Next(Enum.GetValues(typeof(Models.Enums.ProjectStatus)).Length)
                };

                projects.Add(project);

                int taskCount = _random.Next(30, 61);

                for (int t = 1; t <= taskCount; t++)
                {
                    var assignedUser = users[_random.Next(users.Count)];

                    var randTaskName = SampleTaskNames[_random.Next(SampleTaskNames.Length)] + $" #{t}";

                    var startedAt = project.StartDate.Value.AddDays(_random.Next(0, 10));
                    var deadline = startedAt.AddDays(_random.Next(5, 60));

                    var task = new TaskEntity
                    {
                        Id = Guid.NewGuid(),
                        ProjectId = project.Id,
                        UserId = assignedUser.Id,
                        Name = randTaskName,
                        StartedAt = startedAt,
                        Deadline = deadline,
                        Priority = (PriorityStatus)_random.Next(Enum.GetValues(typeof(PriorityStatus)).Length),
                        Status = (TaskEntityStatus)_random.Next(Enum.GetValues(typeof(TaskEntityStatus)).Length)
                    };

                    tasks.Add(task);
                }
            }

            _context.Projects.AddRange(projects);
            _context.Tasks.AddRange(tasks);

            await _context.SaveChangesAsync();
        }
    }
}
