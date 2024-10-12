using System;
using NoteBookmark.Domain;

namespace NoteBookmark.Api;

public interface IDataStorageService
{
	public List<Post> GetFilteredPosts(string filter);

	public Post? GetPost(string rowKey);

	public List<Summary> GetSummaries();

    public void CreateNote(Note note);
}
