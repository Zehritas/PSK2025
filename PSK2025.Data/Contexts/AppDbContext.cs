using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PSK2025.Models.Entities;

namespace PSK2025.Data.Contexts;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {


    }

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

}