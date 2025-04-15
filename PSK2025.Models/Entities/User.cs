using Microsoft.AspNetCore.Identity;

namespace PSK2025.Models.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        // more custom fields if needed etc.
    }
}