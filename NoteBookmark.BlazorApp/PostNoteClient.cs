using System;
using NoteBookmark.Domain;

namespace NoteBookmark.BlazorApp;

public class PostNoteClient(HttpClient httpClient)
{
    public async Task<List<PostL>> GetUnreadPosts()
    {
        var posts = await httpClient.GetFromJsonAsync<List<PostL>>("api/posts");
        return posts ?? new List<PostL>();
    }

    public async Task<List<Summary>> GetSummaries()
    {
        var summaries = await httpClient.GetFromJsonAsync<List<Summary>>("api/summary");
        return summaries ?? new List<Summary>();
    }

    public async Task CreateNote(Note note)
    {
        var response = await httpClient.PostAsJsonAsync("api/notes/note", note);
        response.EnsureSuccessStatusCode();
    }

    public async Task<ReadingNotes> CreateReadingNotes()
    {
        var rnCounter = await httpClient.GetStringAsync("api/settings/GetNextReadingNotesCounter");
        var readingNotes = new ReadingNotes(rnCounter);

        //Get all unused notes 
        var unsortedNotes = await httpClient.GetFromJsonAsync<List<Note>>("api/notes/GetUnUsed");
        
        
        return readingNotes;
    }


    private Dictionary<string, List<Note>> SortNotes(List<Note> notes, string  counter)
    {
        var sortedNotes = new Dictionary<string, List<Note>>();

        foreach (var note in notes)
        {
            var tags = note.Tags!.Split(',');
            
            var category = NoteCategories.GetCategory(tags[0]);
            note.Category = category;
            note.ReadingNotesID = counter;

            if (sortedNotes.ContainsKey(category))
            {
                sortedNotes[category].Add(note);
            }
            else
            {
                sortedNotes.Add(category, new List<Note> {note});
            }
        }

        return sortedNotes;
    }
}
