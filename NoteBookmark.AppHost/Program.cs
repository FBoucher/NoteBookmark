using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<NoteBookmark_Api>("api");

builder.Build().Run();
