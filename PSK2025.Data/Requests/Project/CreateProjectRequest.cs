using PSK2025.Models.DTOs;
using PSK2025.Models.Enums;

namespace PSK2025.Data.Requests.Project;

public record CreateProjectRequest(String Name, string OwnerId, string Description, DateTime? StartDate, DateTime? EndDate);
