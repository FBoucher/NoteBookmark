using Microsoft.Extensions.Azure;
using NoteBookmark.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connStr = Environment.GetEnvironmentVariable("data-storage-connstr")?? string.Empty;

builder.Services.AddTransient<IDataStorageService, DataStorageService>(sp => new DataStorageService(connStr));

var app = builder.Build();

app.MapDefaultEndpoints();

if (string.IsNullOrEmpty(connStr))
{
    app.Logger.LogWarning("Connection string 'data-storage-connstr' is not configured.");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPostEndpoints();
app.MapNoteEndpoints();
app.MapSummaryEndpoints();


app.Run();


