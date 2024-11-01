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
	}

	static async Task<string> GetNextReadingNotesCounter(IDataStorageService dataStorageService)
	{
        var settings = await dataStorageService.GetSettings();
        var counter = settings.ReadingNotesCounter ?? "0";
        return counter;
	}
}
