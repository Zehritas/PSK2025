namespace PSK2025.ApiService.Services.Interfaces;

public interface IUserContextService
{
    string GetCurrentUserId();
    string GetCurrentUsername();
}
