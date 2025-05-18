using Microsoft.AspNetCore.Identity;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Errors;
using PSK2025.Data.Requests;
using PSK2025.Data;
using PSK2025.Models.Enums;
using PSK2025.Data.Repositories.Interfaces;
using PSK2025.Models.Entities;
using PSK2025.Data.Requests.Task;
using Microsoft.EntityFrameworkCore;
using PSK2025.Data.Contexts;
using PSK2025.Data.Responses.Task;
using PSK2025.Models.DTOs;
using TaskEntity = PSK2025.Models.Entities.Task;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;



namespace PSK2025.ApiService.Services;

public class TaskService(
    ITaskRepository taskRepository, 
    IUserProjectRepository userProjectRepository,
    IProjectRepository projectRepository,
    UserManager<User> userManager, 
    IUserContextService userContextService,
    AppDbContext context) : ITaskService
{
    public async Task<Result<Guid>> CreateTaskAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var project = await context.Projects.FindAsync([request.ProjectId], cancellationToken);

        var task = TaskEntity.Create(
            project,
            request.Name,
            null);

        await taskRepository.AddAsync(task, cancellationToken);
        await context.SaveChangesAsync();

        return Result<Guid>.Success(task.Id);
    }

    public async Task<Result> EditTaskAsync(UpdateTaskRequest request, CancellationToken cancellationToken = default)
    {

        var currentTask = await taskRepository.GetByIdAsync(request.TaskId, cancellationToken);

        if (currentTask == null)
        {
            return Result.Failure(TaskErrors.TaskNotFoundError);
        }

        User? user = null;
        if (!string.IsNullOrWhiteSpace(request.UserId))
            user = await userManager.FindByIdAsync(request.UserId);

        currentTask.Update(
            request.Name,
            user,
            request.Deadline,
            request.Status,
            request.PriorityStatus,
            request.FinishedAt
        );

        taskRepository.Update(currentTask);
        await context.SaveChangesAsync();

        return Result.Success();
    }



    public async Task<Result> DeleteTaskByIdAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        var task = await taskRepository.GetByIdAsync(taskId, cancellationToken);
        if (task == null)
            return Result.Failure(TaskErrors.TaskNotFoundError);

        await taskRepository.DeleteAsync(task.Id, cancellationToken);
        await context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<GetTasksResponse>> GetTasksAsync(
        GetTasksRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = userContextService.GetCurrentUserId();
        Guid? projectId = request.ProjectId;
        string? userId = request.UserId;


        if (projectId.HasValue)
        {
            bool currentUserInProject = await taskRepository.IsUserInProjectAsync(
                projectId.Value, currentUserId, cancellationToken);

            if (!currentUserInProject)
                return Result<GetTasksResponse>.Failure(TaskErrors.UserNotInProjectError);

            if (userId != null)
            {
                bool targetUserInProject = await taskRepository.IsUserInProjectAsync(
                    projectId.Value, userId, cancellationToken);

                if (!targetUserInProject)
                    return Result<GetTasksResponse>.Failure(TaskErrors.QueriedUserNotInProjectError);
            }
        }
        else if (userId != null && userId != currentUserId)
        {
            return Result<GetTasksResponse>.Failure(TaskErrors.UserIdMismatchError);
        }

        int skip = (request.Pagination.PageNumber - 1) * request.Pagination.PageSize;
        int take = request.Pagination.PageSize;
        PriorityStatus? priorityStatus = request.Priority;
        TaskEntityStatus? entityStatus = request.Status;


        var taskEntities = await taskRepository.GetUserAccessibleTasksAsync(
            currentUserId,
            projectId,
            userId,
            priorityStatus,
            entityStatus,
            skip,
            take,
            cancellationToken);

        var taskDtos = taskEntities.Select(task => {
        // Create the TaskAssigneeDto if we have user information
        TaskAssigneeDto? assigneeDto = null;
        Console.WriteLine("User null: ");
        if (task.User != null)
        {
            Console.WriteLine("User not null: ");
            Console.WriteLine(task.User.UserName);
            assigneeDto = new TaskAssigneeDto(
                task.User.Id,
                task.User.FirstName ?? string.Empty,
                task.User.LastName ?? string.Empty
            );
        }
        
        return new TaskDto(
            task.Id,
            assigneeDto, // Pass the assignee dto
            task.Name,
            task.StartedAt,
            task.FinishedAt,
            task.Deadline,
            task.Status,
            task.Priority
        );
    }).ToList();

        var response = new GetTasksResponse(taskDtos);
        return Result<GetTasksResponse>.Success(response);
    }


}
