using PSK2025.ApiService.Interfaces;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.UserProject;

public class UserProjectRouteGroup(IEnumerable<IEndpoint> endpoints) : IRouteGroup
{
    public RouteGroupName Group => RouteGroupName.UserProject;
    public string RoutePrefix => "/api/user-project";
    public string Tag => "User Project";
}