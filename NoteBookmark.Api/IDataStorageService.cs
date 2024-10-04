using System;

namespace NoteBookmark.Api;

public interface IDataStorageService
{
	public List<Post> GetFilteredPosts(string filter);

	public Post? GetPost(string rowKey);
}
