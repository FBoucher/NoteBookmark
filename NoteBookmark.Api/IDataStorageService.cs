using System;
using NoteBookmark.Domain;
using static NoteBookmark.Api.DataStorageService;

namespace NoteBookmark.Api;

public interface IDataStorageService
{
	public List<PostL> GetFilteredPosts(string filter);
		
	public Post? GetPost(string rowKey);

	public List<Summary> GetSummaries();

	public List<Note> GetUnUsedNotes();
    public void CreateNote(Note note);

	public Task<Settings> GetSettings();
}
