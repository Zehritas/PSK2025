using PSK2025.Data;
using PSK2025.Models.Entities;
using PSK2025.Data.Requests.Task;
using PSK2025.Data.Responses.Task;
using PSK2025.Data.Requests;

namespace PSK2025.ApiService.Services.Interfaces;

public interface ITaskService
{
    Task<Result<Guid>> CreateTaskAsync(CreateTaskRequest request, CancellationToken cancellationToken = default);
    Task<Result> EditTaskAsync(UpdateTaskRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteTaskByIdAsync(Guid taskId, CancellationToken cancellationToken = default);
    Task<Result<GetTasksResponse>> GetTasksAsync(GetTasksRequest request, CancellationToken cancellationToken = default);
}
