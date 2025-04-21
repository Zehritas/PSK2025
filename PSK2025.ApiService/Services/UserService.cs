using PSK2025.Models.DTO;

namespace PSK2025.ApiService.Services
{
    public class UserService : IUserService
    {
        public async Task<bool> RegisterAsync(RegisterRequest request)
        {

            return await Task.FromResult(true);
        }

        public async Task<string?> LoginAsync(LoginRequest request)
        {

            if (!string.IsNullOrWhiteSpace(request.Email) && !string.IsNullOrWhiteSpace(request.Password))
            {
                return await Task.FromResult($"mock-token-for-{request.Email}");
            }

            return await Task.FromResult<string?>(null);
        }
    }
}
