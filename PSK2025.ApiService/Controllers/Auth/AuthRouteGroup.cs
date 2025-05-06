using PSK2025.ApiService.Interfaces;
using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Controllers.Auth;

public class AuthRouteGroup(IEnumerable<IEndpoint> endpoints) : IRouteGroup
{
    public RouteGroupName Group => RouteGroupName.Auth;
    public string RoutePrefix => "/api/auth";
    public string Tag => "Auth";
}