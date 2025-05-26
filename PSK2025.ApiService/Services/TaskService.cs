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
        var task = new TaskEntity
        {
            ProjectId = request.ProjectId,
            UserId = request.UserId,
            Name = request.Name,
            Deadline = request.Deadline,
            Priority = request.Priority,
            Status = request.Status,
            StartedAt = request.StartedAt,
            FinishedAt = request.FinishedAt,
        };

        await taskRepository.AddAsync(task, cancellationToken);
        await context.SaveChangesAsync();

        return Result<Guid>.Success(task.Id);
    }

    public async Task<Result> EditTaskAsync(
        UpdateTaskRequest request,
        bool bypassConcurrency = false,
        CancellationToken cancellationToken = default)
    {
        var currentTask = await taskRepository.GetByIdAsync(request.TaskId, cancellationToken);

        if (currentTask == null)
        {
            return Result.Failure(TaskErrors.TaskNotFoundError);
        }

        if (currentTask.Version != request.Version && !bypassConcurrency)
        {
            return Result<TaskDto>.Failure(ConcurrencyErrors.OptimisticLockingError);
        }

        currentTask.Update(
            request.Name,
            request.UserId,
            request.Deadline,
            request.Status,
            request.Priority,
            request.FinishedAt,
            request.StartedAt
        );

        taskRepository.Update(currentTask);

        await context.SaveChangesAsync(cancellationToken);

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

    public async Task<TaskDto> GetByIdAsync(Guid id, CancellationToken token)
    {
        var userId = userContextService.GetCurrentUserId();

        var entity = await context.Tasks
                                  .Include(t => t.Project)
                                  .Include(t => t.User)
                                  .Where(p => p.UserId == userId
                                           || p.Project.UserProjects.Any(up => up.UserId == userId)
                                           || p.Project.OwnerId == userId)
                                  .Where(p => p.Id == id)
                                  .FirstOrDefaultAsync(cancellationToken: token);

        if (entity == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        return new TaskDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Status = entity.Status,
            Assignee = entity.User != null ? new TaskAssigneeDto(entity.User.Id, entity.User.FirstName, entity.User.LastName) : null,
            StartedAt = entity.StartedAt,
            FinishedAt = entity.FinishedAt,
            Deadline = entity.Deadline,
            Priority = entity.Priority,
            Project = new ProjectDto
            {
                Name = entity.Project.Name
            },
            Version = entity.Version,
        };
    }

    public async Task<PaginatedResult<TaskDto>> GetTasksAsync(
        GetTasksRequest request,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, request.Pagination.PageNumber);
        var pageSize = Math.Clamp(request.Pagination.PageSize, 1, 1000);
        var userId = userContextService.GetCurrentUserId();

        var query = context.Tasks
                           .Include(p => p.User)
                           .Include(p => p.Project)
                           .Where(p => p.UserId == userId
                                    || p.Project.UserProjects.Any(up => up.UserId == userId)
                                    || p.Project.OwnerId == userId);

        if (request.Priority.HasValue)
        {
            query = query.Where(p => p.Priority == request.Priority);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(p => p.Status == request.Status);
        }

        if (request.ProjectId.HasValue)
        {
            query = query.Where(p => p.ProjectId == request.ProjectId);
        }

        if (request.UserId != null)
        {
            query = query.Where(p => p.UserId == request.UserId);
        }

        var totalCount = await query.CountAsync();

        var items = await query
                          .OrderBy(p => p.StartedAt)
                          .Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .Select(t => new TaskDto
                          {
                              Id = t.Id,
                              Name = t.Name,
                              Status = t.Status,
                              Assignee = t.User != null ? new TaskAssigneeDto(t.User.Id, t.User.FirstName, t.User.LastName) : null,
                              StartedAt = t.StartedAt,
                              FinishedAt = t.FinishedAt,
                              Deadline = t.Deadline,
                              Priority = t.Priority,
                              Project = new ProjectDto
                              {
                                  Name = t.Project.Name
                              },
                              Version = t.Version,
                          })
                          .ToListAsync();

        return new PaginatedResult<TaskDto>
        {
            Items = items,
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize
        };
    }
}
