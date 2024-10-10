using Microsoft.Extensions.Configuration;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var connectionString = builder.AddConnectionString("data-storage-connstr");

var api = builder.AddProject<NoteBookmark_Api>("api")
            .WithEnvironment("data-storage-connstr",connectionString);

builder.AddProject<NoteBookmark_BlazorApp>("blazor-app")
    .WithReference(api);

builder.Build().Run();
