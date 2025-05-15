using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PSK2025.Models.Entities;
using TaskEntity = PSK2025.Models.Entities.Task;


namespace PSK2025.Data.Contexts;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<Models.Entities.Task> Tasks { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;

    public DbSet<UserProject> UserProjects { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserProject>()
            .HasKey(up => new { up.UserId, up.ProjectId });

        // Relationships
        modelBuilder.Entity<UserProject>()
            .HasOne(up => up.User)
            .WithMany(u => u.UserProjects)
            .HasForeignKey(up => up.UserId);

        modelBuilder.Entity<UserProject>()
            .HasOne(up => up.Project)
            .WithMany(p => p.UserProjects)
            .HasForeignKey(up => up.ProjectId);

 
        modelBuilder.Entity<TaskEntity>()
            .HasOne<Project>()
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId);

        modelBuilder.Entity<TaskEntity>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .IsRequired(false); // since it's nullable

 
    }
}
