using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<NoteBookmark_Api>("api");

builder.AddProject<NoteBookmark_BlazorApp>("blazorApp")
    .WithReference(api);    

builder.Build().Run();
