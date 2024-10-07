using System;

namespace NoteBookmark.BlazorApp;

public class PostNoteClient(HttpClient httpClient)
{


    public async Task<List<Post>> GetUnreadPosts()
    {
        var posts = await httpClient.GetFromJsonAsync<List<Post>>("api/posts");
        return posts ?? new List<Post>();
    }
}
