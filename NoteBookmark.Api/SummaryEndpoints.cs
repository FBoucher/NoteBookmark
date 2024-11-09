using Microsoft.AspNetCore.Http.HttpResults;
using NoteBookmark.Domain;

namespace NoteBookmark.Api;

public static class SummaryEndpoints
{
	public static void MapSummaryEndpoints(this IEndpointRouteBuilder app)
	{
		var endpoints = app.MapGroup("api/summary")
				.WithOpenApi();

		endpoints.MapGet("/", GetSummaries)
			.WithDescription("Get all unread posts");

		endpoints.MapPost("/summary", SaveSummary)
			.WithDescription("Create or update the summary");
	}
	static List<Summary> GetSummaries(IDataStorageService dataStorageService)
	{
		return dataStorageService.GetSummaries();
	}

	static Results<Created<Summary>, BadRequest> SaveSummary(Summary summary, IDataStorageService dataStorageService)
	{
		try
		{
			var response = dataStorageService.SaveSummary(summary);
			return TypedResults.Created($"/api/summary/{summary.RowKey}", summary);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred while saving the summary: {ex.Message}");
			return TypedResults.BadRequest();
		}
	}


}
