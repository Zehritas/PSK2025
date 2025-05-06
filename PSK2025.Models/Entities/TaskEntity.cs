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
    
    public DateTime? Deadline { get;  set; }
    public TaskEntityStatus Status { get;  set; }
    


}