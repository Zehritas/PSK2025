using Microsoft.AspNetCore.Builder;
using PSK2025.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace PSK2025.MigrationService.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<AppDbContext>>();
            logger.LogError(ex, "An error occurred while applying migrations.");
            throw;
        }
    }
}
