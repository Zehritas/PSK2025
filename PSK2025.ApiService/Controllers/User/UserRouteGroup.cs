using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.User;
using PSK2025.ApiService.Interfaces;

public class UserRouteGroup(IEnumerable<IEndpoint> endpoints) : IRouteGroup
{
    public RouteGroupName Group => RouteGroupName.User;
    public string RoutePrefix => "/api/users";
    public string Tag => "User";
}