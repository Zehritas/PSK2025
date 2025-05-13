using Microsoft.AspNetCore.Identity;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.Data.Errors;
using PSK2025.Data.Requests;
using PSK2025.Data;
using PSK2025.MigrationService.Abstractions;
using PSK2025.Data.Repositories.Interfaces;
using PSK2025.Models.Entities;
using PSK2025.Data.Requests.Task;
using Microsoft.EntityFrameworkCore;
using PSK2025.Data.Contexts;
using PSK2025.Data.Responses.Task;
using PSK2025.Models.DTOs;
using TaskEntity = PSK2025.Models.Entities.Task;


namespace PSK2025.ApiService.Services;

public class TaskService(
    ITaskRepository taskRepository, 
    UserManager<User> userManager, 
    IUserContextService userContextService,
    AppDbContext context) : ITaskService
{
    public async Task<Result<Guid>> CreateTaskAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
    {

        var task = TaskEntity.Create(
            request.ProjectId,
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

        currentTask.Update(
            request.Name,
            request.UserId,
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

    public async Task<Result<GetTasksResponse>> GetTasksAsync(GetProjectTasksRequest request, CancellationToken cancellationToken = default)
    {
        var tasks = await taskRepository.GetListAsync(
            request.ProjectId,
            (request.Pagination.PageNumber - 1) * request.Pagination.PageSize,
            request.Pagination.PageSize,
            cancellationToken
        );

        if (!tasks.Any())
            return Result<GetTasksResponse>.Failure(TaskErrors.NoTasksFoundError);

        var taskDtos = tasks.Select(task => new TaskDto(
            task.Id,               
            task.UserId ?? string.Empty, 
            task.Name,             
            task.StartedAt,        
            task.FinishedAt,       
            task.Deadline,
            task.Status,
            task.Priority)).ToList();

        var response = new GetTasksResponse(taskDtos);

        return Result<GetTasksResponse>.Success(response);
    }
}
