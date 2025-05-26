using PSK2025.Models.Enums;


namespace PSK2025.Data.Requests.Task;

public record CreateTaskRequest(
    Guid ProjectId,
    string Name,
    TaskEntityStatus Status,
    PriorityStatus Priority,
    DateTime? Deadline,
    string? UserId,
    DateTime? StartedAt,
    DateTime? FinishedAt
);
