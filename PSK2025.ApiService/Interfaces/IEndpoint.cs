namespace PSK2025.ApiService.Interfaces;
using PSK2025.Data.Enums;

public interface IEndpoint
{
    RouteGroupName Group { get; }
    void MapEndpoints(RouteGroupBuilder group);
}
