using PSK2025.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Models.DTOs;

public record TaskDto(
    Guid Id,
    TaskAssigneeDto? Assignee,
    string Name,
    DateTime StartedAt,
    DateTime? FinishedAt,
    DateTime? Deadline,
    TaskEntityStatus Status,
    PriorityStatus? Priority
);