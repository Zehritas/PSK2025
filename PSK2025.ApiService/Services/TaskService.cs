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

        currentTask.Update(
            request.Name,
            request.UserId,
            request.Deadline,
            request.Status,
            request.PriorityStatus,
            request.FinishedAt
        );

        taskRepository.Update(currentTask);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var entry = ex.Entries.Single();
            var databaseValues = await entry.GetDatabaseValuesAsync();

            if (databaseValues == null)
                return Result.Failure(TaskErrors.TaskNotFoundError);

            var databaseEntity = (TaskEntity)databaseValues.ToObject();

            if (bypassConcurrency)
            {
                databaseEntity.Update(
                    request.Name,
                    request.UserId,
                    request.Deadline,
                    request.Status,
                    request.PriorityStatus,
                    request.FinishedAt
                );

                entry.OriginalValues.SetValues(databaseValues);
                entry.CurrentValues.SetValues(databaseEntity);

                await context.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            else
            {
                return Result<TaskDto>.Failure(ConcurrencyErrors.OptimisticLockingError);
            }
        }
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
                                   .Where(p => p.UserId == userId || p.Project.UserProjects.Any(up => up.UserId == userId) || p.Project.OwnerId == userId)
                                   .Where(p => p.Id == id)
                                   .FirstOrDefaultAsync(cancellationToken:token);

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
        };
    }
    
    public async Task<PaginatedResult<TaskDto>> GetTasksAsync(
        GetTasksRequest request,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, request.Pagination.PageNumber);
        var pageSize = Math.Clamp(request.Pagination.PageSize, 1, 50);
        var userId = userContextService.GetCurrentUserId();

        var query = context.Tasks
                           .Include(p => p.User)
                           .Include(p => p.Project)
                           .Where(p => p.UserId == userId || p.Project.UserProjects.Any(up => up.UserId == userId) || p.Project.OwnerId == userId);

        int skip = (request.Pagination.PageNumber - 1) * request.Pagination.PageSize;
        int take = request.Pagination.PageSize;

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
                          .Select(p => new TaskDto
                          {
                              Id = p.Id,
                              Name = p.Name,
                              Status = p.Status,
                              Assignee = p.User != null ? new TaskAssigneeDto(p.User.Id, p.User.FirstName, p.User.LastName) : null,
                              StartedAt = p.StartedAt,
                              FinishedAt = p.FinishedAt,
                              Deadline = p.Deadline,
                              Priority = p.Priority,
                              Project = new ProjectDto{Name = p.Project.Name},
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
