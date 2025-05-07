using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using PSK2025.Models.Enums;

namespace PSK2025.Models.Entities;

public class TaskEntity
{

    public TaskEntity() { }
    private TaskEntity(
      Guid id,
      Guid projectid,
      String? userid,
      String name,
      DateTime? deadline = null)
    {
        Id = id;
        Projectid = projectid;
        UserId = userid;
        Name = name;
        StartedAt = DateTime.UtcNow;
        Status = TaskEntityStatus.ToBeDone;
        Deadline = deadline;
    }

    public Guid Id { get; private set; }
    public Guid Projectid { get; private set; }
    public String? UserId { get; private set; }
    public String Name { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public DateTime? Deadline { get; set; }
    public TaskEntityStatus Status { get; private set; }

    public static TaskEntity Create(Guid projectId, string name, string? userId = null, DateTime? deadline = null)
    {
        var task = new TaskEntity(Guid.NewGuid(), projectId, userId, name)
        {
            Deadline = deadline
        };

        return task;
    }

    public void Update(string? name, string? userId, DateTime? deadline, TaskEntityStatus status, DateTime? finishedAt)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;

        UserId = userId;
        Deadline = deadline;
        Status = status;

        if (status == TaskEntityStatus.Complete)
            FinishedAt = finishedAt ?? DateTime.UtcNow;
        else
            FinishedAt = null;
    }
}