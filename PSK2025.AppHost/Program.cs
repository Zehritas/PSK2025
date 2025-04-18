using Projects;

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

builder.AddProject<Projects.PSK2025_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
