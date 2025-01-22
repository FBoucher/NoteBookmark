using System;
using Microsoft.AspNetCore.Http.HttpResults;
using NoteBookmark.Domain;
using static NoteBookmark.Api.DataStorageService;

namespace NoteBookmark.Api;

public static class PostEndpoints
{
	public static void MapPostEndpoints(this IEndpointRouteBuilder app)
	{
		var endpoints = app.MapGroup("api/posts")
				.WithOpenApi();

		endpoints.MapGet("/", GetUnreadPosts)
			.WithDescription("Get all unread posts");
		endpoints.MapGet("/{id}", Get)
			.WithDescription("Get a post by id");
		endpoints.MapPost("/", SavePost)
			.WithDescription("Save or Create a post");
		endpoints.MapPost("/extractPostDetails", ExtractPostDetails)
			.WithDescription("Extract post details from URL and save the post");
	}
	
	static List<PostL> GetUnreadPosts(IDataStorageService dataStorageService)
	{
		return dataStorageService.GetFilteredPosts("is_read eq false");
	}

	static Results<Ok<Post>, NotFound> Get(string id, IDataStorageService dataStorageService)
	{  
		var post = dataStorageService.GetPost(id);
		return post is null ? TypedResults.NotFound() : TypedResults.Ok(post);
	}

	static Results<Ok, BadRequest> SavePost(Post post, IDataStorageService dataStorageService)
	{
		if (dataStorageService.SavePost(post))
		{
			return TypedResults.Ok();
		}
		return TypedResults.BadRequest();
	}

	static async Task<Results<Ok<Post>, BadRequest>> ExtractPostDetails(string url, IDataStorageService dataStorageService)
	{
		try
		{
			var post = await dataStorageService.ExtractPostDetailsFromUrl(url);
			if (post != null)
			{
				dataStorageService.SavePost(post);
				return TypedResults.Ok(post);
			}
			return TypedResults.BadRequest();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred while extracting post details: {ex.Message}");
			return TypedResults.BadRequest();
		}
	}
}
