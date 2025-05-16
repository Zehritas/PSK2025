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

    public static readonly Error UserNotInProjectError = new("Task.UserNotInProject", "You are not a member of this project", HttpStatusCode.Forbidden);

    public static readonly Error QueriedUserNotInProjectError = new("Task.QueriedUserNotInProject", "The specified user is not assigned to the project", HttpStatusCode.BadRequest);

    public static readonly Error UserIdMismatchError = new("Task.UserIdMismatch", "The provided user ID does not match the current authenticated user.", HttpStatusCode.Forbidden);
}


