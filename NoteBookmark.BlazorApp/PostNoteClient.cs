using System;
using NoteBookmark.Domain;

namespace NoteBookmark.BlazorApp;

public class PostNoteClient(HttpClient httpClient)
{


    public async Task<List<Post>> GetUnreadPosts()
    {
        var posts = await httpClient.GetFromJsonAsync<List<Post>>("api/posts");
        return posts ?? new List<Post>();
    }

    public async Task<List<Summary>> GetSummaries()
    {
        var summaries = await httpClient.GetFromJsonAsync<List<Summary>>("api/summary");
        return summaries ?? new List<Summary>();
    }
}
