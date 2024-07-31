var builder = DistributedApplication.CreateBuilder(args);

var postgresServer = builder.AddPostgres("postgres", port: 5432)
    .WithDataVolume()
    .WithPgAdmin(c => c.WithHostPort(10000))
    .PublishAsAzurePostgresFlexibleServer();

var postgresDb = postgresServer.AddDatabase("api-db");

var apiService = builder
    .AddProject<Projects.MyAspireApp_ApiService>("apiservice")
    .WithReference(postgresDb, "Database");

builder.AddProject<Projects.MyAspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();