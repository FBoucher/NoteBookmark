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
        var rnCounter = await httpClient.GetStringAsync("api/settings/GetNextReadingNotesCounter");
        note.PartitionKey = rnCounter;
        var response = await httpClient.PostAsJsonAsync("api/notes/note", note);
        response.EnsureSuccessStatusCode();
    }

    public async Task<ReadingNotes> CreateReadingNotes()
    {
        var rnCounter = await httpClient.GetStringAsync("api/settings/GetNextReadingNotesCounter");
        var readingNotes = new ReadingNotes(rnCounter);

        //Get all unused notes 
        var unsortedNotes = await httpClient.GetFromJsonAsync<List<Note>>($"api/notes/GetNotesForSummary/{rnCounter}");
        

        readingNotes.Notes = GroupNotesByCategory(unsortedNotes ?? new List<Note>());
        
        return readingNotes;
    }


    private Dictionary<string, List<Note>> GroupNotesByCategory(List<Note> notes)
    {
        var sortedNotes = new Dictionary<string, List<Note>>();

        foreach (var note in notes)
        {
            var tags = note.Tags!.Split(',');
            
            if(string.IsNullOrEmpty(note.Category)){
                note.Category = NoteCategories.GetCategory(tags[0]);
            }

            string category = note.Category;
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
