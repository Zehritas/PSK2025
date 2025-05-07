using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Interfaces;

public interface IRouteGroup
{
    RouteGroupName Group { get; }
    string RoutePrefix { get; }
    string Tag { get; }
}