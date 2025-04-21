using Microsoft.EntityFrameworkCore;
using PSK2025.Data.Contexts;
using PSK2025.MigrationService;
using PSK2025.MigrationService.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresdb"))
);



var host = builder.Build();
await host.ApplyMigrations();
host.Run();

