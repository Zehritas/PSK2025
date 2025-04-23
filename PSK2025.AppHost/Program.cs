using Microsoft.Extensions.Diagnostics.HealthChecks;
using Projects;
using System.Diagnostics;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false);;

var postgresdb = postgres.AddDatabase("postgresdb");

var cache = builder.AddRedis("cache");

builder.AddProject<PSK2025_MigrationService>("migrations")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

var apiService = builder.AddProject<Projects.PSK2025_ApiService>("apiservice")
    .WithReference(postgresdb);

apiService.WithCommand(
    "swagger-ui", 
    "Swagger UI documentation", 
    executeCommand: async _ =>
    {
        var endpoint = apiService.GetEndpoint("https");
        var url = $"{endpoint.Url}/swagger";

        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

        return new ExecuteCommandResult { Success = true }; 
    },
    updateState: context => context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy
        ? ResourceCommandState.Enabled
        : ResourceCommandState.Disabled, 
    iconName: "Document", 
    iconVariant: IconVariant.Filled 
);

builder.Build().Run();
