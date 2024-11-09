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
		
		endpoints.MapGet("/", GetNotes)
			.WithDescription("Get all unused reading notes with with the info about the related post.");

		endpoints.MapGet("/GetNotesForSummary/{ReadingNotesId}", GetNotesForSummary)
			.WithDescription("Get all notes with the info about the related post for a specific reading notes summary.");

		endpoints.MapPost("/SaveReadingNotes", SaveReadingNotes)
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

	static Results<Ok<List<Note>>, NotFound> GetNotes(IDataStorageService dataStorageService)
	{
		var notes = dataStorageService.GetNotes();
		return notes == null ? TypedResults.NotFound() : TypedResults.Ok(notes);
	}

	static Results<Ok<List<ReadingNote>>, NotFound> GetNotesForSummary(string ReadingNotesId, IDataStorageService dataStorageService)
	{
		var notes = dataStorageService.GetNotesForSummary(ReadingNotesId);
		return notes == null ? TypedResults.NotFound() : TypedResults.Ok(notes);
	}

	static Results<Created, BadRequest> SaveReadingNotes(ReadingNotes readingNotes, IDataStorageService dataStorageService)
	{
		try
		{
			var url = dataStorageService.SaveReadingNotes(readingNotes).Result;
			return url == null ? TypedResults.BadRequest() : TypedResults.Created(url);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred while creating a note: {ex.Message}");
			return TypedResults.BadRequest();
		}
	}

}
