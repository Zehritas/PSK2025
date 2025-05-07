using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using PSK2025.Models.Enums;

namespace PSK2025.Models.Entities;

public class TaskEntity
{
    private TaskEntity(
      Guid id,
      Guid projectid,
      Guid? userid,
      String name)
    {
        Id = id;
        Projectid = projectid;
        Userid = userid;
        Name = name;
        StartedAt = DateTime.Now;
        Status = TaskEntityStatus.ToBeDone;
    }

    public Guid Id { get; }
    public Guid Projectid { get; private set; }
    public Guid? Userid { get; private set; }
    public String Name { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public TaskEntityStatus Status { get; private set; }

    public void Update(DateTime startedAt, DateTime? finishedAt, TaskEntityStatus taskStatus)
    {
        StartedAt = startedAt;
        FinishedAt = finishedAt ?? null;
        Status = taskStatus;
    }


}
