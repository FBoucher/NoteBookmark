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
	}
	static List<Summary> GetSummaries(IDataStorageService dataStorageService)
	{
		return dataStorageService.GetSummaries();
	}
}
