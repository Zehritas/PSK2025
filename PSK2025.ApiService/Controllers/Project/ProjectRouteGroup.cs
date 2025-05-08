using PSK2025.ApiService.Interfaces;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Project;

public class ProjectRouteGroup(IEnumerable<IEndpoint> endpoints) : IRouteGroup
{
    public RouteGroupName Group => RouteGroupName.Project;
    public string RoutePrefix => "/api/project";
    public string Tag => "Project";
}