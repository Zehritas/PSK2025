using PSK2025.Models.Enums;

namespace PSK2025.Models.DTOs;

// Not sure if dto's are needed if we use req/response
// Ill just do it, but we can maybe adjust this later

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ProjectStatus Status { get; set; }
}