using System;
using NoteBookmark.Domain;
using static NoteBookmark.Api.DataStorageService;

namespace NoteBookmark.Api;

public interface IDataStorageService
{
	public List<PostL> GetFilteredPosts(string filter);
		
	public Post? GetPost(string rowKey);

	public List<Summary> GetSummaries();

	List<ReadingNote> GetNotesForSummary(string PartitionKey);
    public void CreateNote(Note note);

	public List<Note> GetNotes();

	public Task<Settings> GetSettings();

	public Task<string> SaveReadingNotes(ReadingNotes readingNotes);

	public Task<bool> SaveSummary(Summary summary);
}
