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
    public Guid Id { get; set; }
    public Guid Projectid { get;  set; }
    public string UserId { get; set; } = null!;
    public String Name { get;  set; }
    public DateTime StartedAt { get;  set; }
    public DateTime? FinishedAt { get;  set; }
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
    public DateTime? Deadline { get; set; }
    public TaskEntityStatus Status { get; private set; }
    


}