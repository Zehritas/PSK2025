using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Interfaces;

public interface IEndpoint
{
    RouteGroupName Group { get; }
    void MapEndpoints(RouteGroupBuilder group);
}
