using PSK2025.Models.Enums;

namespace PSK2025.ApiService.Interfaces;
using PSK2025.Models.Enums;

public interface IEndpoint
{
    RouteGroupName Group { get; }
    void MapEndpoints(RouteGroupBuilder group);
}
