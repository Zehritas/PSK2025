using PSK2025.Models.DTO;
using System.Threading.Tasks;

namespace PSK2025.ApiService.Services
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<string?> LoginAsync(LoginRequest request);
    }
}
