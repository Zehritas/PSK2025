using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using PSK2025.Models.Enums;

namespace PSK2025.Models.Entities;

public class Task
{
    public Task() { }
    private Task(
      Guid id,
      Guid projectid,
      String? userid,
      String name,
      DateTime? deadline = null)
    {
        Id = id;
        ProjectId = projectid;
        UserId = userid;
        Name = name;
        StartedAt = DateTime.UtcNow;
        Status = TaskEntityStatus.NotStarted;
        Deadline = deadline;
    }

    public Guid Id { get; private set; }
    public Guid ProjectId { get; set; }
    public String? UserId { get; set; }
    public String Name { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public DateTime? Deadline { get; set; }

    public PriorityStatus? Priority { get; private set; }
    public TaskEntityStatus Status { get; private set; }

    public virtual Project Project { get; set; } = null!;
    public virtual User? User { get; set; }

    public static Task Create(Guid projectId, string name, string? userId = null, DateTime? deadline = null)
    {
        userId = string.IsNullOrWhiteSpace(userId) ? null : userId;

        var task = new Task(Guid.NewGuid(), projectId, userId, name)
        {
            Deadline = deadline
        };

        return task;
    }

    public void Update(string? name, string? userId, DateTime? deadline, TaskEntityStatus status, PriorityStatus priority, DateTime? finishedAt)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;

        UserId = userId;
        Deadline = deadline;
        Status = status;
        Priority = priority;

        if (status == TaskEntityStatus.Completed)
            FinishedAt = finishedAt ?? DateTime.UtcNow;
        else
            FinishedAt = null;
    }
}