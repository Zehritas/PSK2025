using PSK2025.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Models.DTOs;

public record TaskDto
{
    public Guid Id { get; set; }
    public TaskAssigneeDto? Assignee { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; } 
    public DateTime? Deadline { get; set; }
    public TaskEntityStatus Status { get; set; }
    public PriorityStatus? Priority { get; set; }
    public ProjectDto Project { get; set; } = null!;
    public uint Version { get; set; }
}
