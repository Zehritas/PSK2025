using PSK2025.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Requests.Task;

public class UpdateTaskRequest
{
    [Required(ErrorMessage = "Task ID is required.")]
    public Guid TaskId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = default!;

    public string? UserId { get; set; }

    public DateTime? Deadline { get; set; }

    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public TaskEntityStatus Status { get; set; }

    public PriorityStatus Priority { get; set; }

    public uint Version { get; set; }
}
