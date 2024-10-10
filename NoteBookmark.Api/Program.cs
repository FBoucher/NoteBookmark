using Microsoft.Extensions.Azure;
using NoteBookmark.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connStr = builder.Configuration.GetConnectionString("data-storage-connstr");
if (string.IsNullOrEmpty(connStr))
{
    throw new InvalidOperationException("Connection string 'data-storage-connstr' is not configured.");
}
builder.Services.AddTransient<IDataStorageService, DataStorageService>(sp => new DataStorageService(connStr));

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapPostEndpoints();


app.Run();


