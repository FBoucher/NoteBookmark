using System;
using Microsoft.AspNetCore.Http.HttpResults;
using NoteBookmark.Domain;

namespace NoteBookmark.Api;

public static class NoteEnpoints
{
    public static void MapNoteEndpoints(this IEndpointRouteBuilder app)
	{
		var endpoints = app.MapGroup("api/notes")
				.WithOpenApi();

		endpoints.MapPost("/note", CreateNote)
			.WithDescription("Create a new note");
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
