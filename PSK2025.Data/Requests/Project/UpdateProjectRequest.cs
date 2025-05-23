using PSK2025.Models.Enums;

namespace PSK2025.Data.Requests.Project;

public record UpdateProjectRequest
{
    public string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string? Description { get; set; }
    public ProjectStatus Status { get; set; }
};
