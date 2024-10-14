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
		endpoints.MapPost("/notes", CreateNote)
			.WithDescription("Create a new note");
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

	static Results<Created<Note>, BadRequest> CreateNote(Note note, IDataStorageService dataStorageService)
	{
		try
		{
			dataStorageService.CreateNote(note);
			return TypedResults.Created($"/api/notes/{note.RowKey}", note);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred while creating a note: {ex.Message}");
			return TypedResults.BadRequest();
		}
	}
}
