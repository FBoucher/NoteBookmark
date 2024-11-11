using System;
using NoteBookmark.Domain;

namespace NoteBookmark.Api;

public static class SettingEndpoints
{
    public static void MapSettingEndpoints(this IEndpointRouteBuilder app)
	{
		var endpoints = app.MapGroup("api/settings")
				.WithOpenApi();

		endpoints.MapGet("/GetNextReadingNotesCounter", GetNextReadingNotesCounter)
			.WithDescription("Get the next reading notes counter (aka number)");

        endpoints.MapPost("/SaveSettings", SaveSettings)
            .WithDescription("Save updated settings");
	}

	static async Task<string> GetNextReadingNotesCounter(IDataStorageService dataStorageService)
	{
        var settings = await dataStorageService.GetSettings();
        var counter = settings.ReadingNotesCounter ?? "0";
        return counter;
	}

    static async Task<Results<Ok, BadRequest>> SaveSettings(Settings settings, IDataStorageService dataStorageService)
    {
        try
        {
            var result = await dataStorageService.SaveSettings(settings);
            return result ? TypedResults.Ok() : TypedResults.BadRequest();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving settings: {ex.Message}");
            return TypedResults.BadRequest();
        }
    }
}
