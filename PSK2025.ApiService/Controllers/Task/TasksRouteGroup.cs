using PSK2025.ApiService.Interfaces;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Task;

public class TasksRouteGroup(IEnumerable<IEndpoint> endpoints) : IRouteGroup
{
    public RouteGroupName Group => RouteGroupName.Task;
    public string RoutePrefix => "/api/task";
    public string Tag => "Task";
}
