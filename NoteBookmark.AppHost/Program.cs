using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<NoteBookmark_Api>("api");

builder.AddProject<NoteBookmark_BlazorApp>("blazor-app")
    .WithReference(api);    

builder.Build().Run();
