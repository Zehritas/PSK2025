using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Errors;

public static class TaskErrors
{
    public static readonly Error TaskNotFoundError = new Error("Task.NotFound", "Task not found", HttpStatusCode.NotFound);
    public static readonly Error NoTasksFoundError = new Error("Task.NoneFound", "No tasks found", HttpStatusCode.NotFound);
}


