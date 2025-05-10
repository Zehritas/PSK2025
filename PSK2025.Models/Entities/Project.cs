using PSK2025.Models.Enums;

namespace PSK2025.Models.Entities;

public class Project
{
    
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ProjectStatus Status { get; set; }

    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();

}