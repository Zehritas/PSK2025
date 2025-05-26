using Microsoft.AspNetCore.Identity;
using PSK2025.Models.Entities;

namespace PSK2025.Models.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        // more custom fields if needed etc.
        
        public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        private readonly List<RefreshToken> _refreshTokens = new();

        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public void AddRefreshToken(RefreshToken refreshToken)
        {
            _refreshTokens.Add(refreshToken);
        }
    }
}