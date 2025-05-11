using PSK2025.Models.DTOs;
using PSK2025.Models.Enums;

namespace PSK2025.Data.Requests.Project;

public record CreateProjectRequest(String ProjectName, string OwnerId, DateTime? StartDate, DateTime? EndDate);
