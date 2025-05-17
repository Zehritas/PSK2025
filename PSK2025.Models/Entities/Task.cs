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
    private Task(Guid id, string name, Project project, User? user = null, DateTime? deadline = null)
    {
        Id = id;
        Name = name;
        Project = project;
        User = user;
        StartedAt = DateTime.UtcNow;
        Status = TaskEntityStatus.NotStarted;
        Deadline = deadline;
    }

    public Guid Id { get; private set; }
    public String Name { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public DateTime? Deadline { get; set; }

    public PriorityStatus? Priority { get; private set; }
    public TaskEntityStatus Status { get; private set; }

    public virtual Project Project { get; set; } = null!;
    public virtual User? User { get; set; }

    public static Task Create(Project project, string name, User? user = null, DateTime? deadline = null)
    {
        var task = new Task(Guid.NewGuid(), name, project, user)
        {
            Deadline = deadline
        };

        return task;
    }

    public void Update(string? name, User? user, DateTime? deadline, TaskEntityStatus status, PriorityStatus priority, DateTime? finishedAt)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;

        User = user;
        Deadline = deadline;
        Status = status;
        Priority = priority;

        FinishedAt = status == TaskEntityStatus.Completed ? finishedAt ?? DateTime.UtcNow : null;
    }
}