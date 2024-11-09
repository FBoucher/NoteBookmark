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
        var unsortedNotes = await httpClient.GetFromJsonAsync<List<ReadingNote>>($"api/notes/GetNotesForSummary/{rnCounter}");
        
        if(unsortedNotes == null || unsortedNotes.Count == 0){
            return readingNotes;
        }

        Dictionary<string, List<ReadingNote>> sortedNotes = GroupNotesByCategory(unsortedNotes);

        readingNotes.Notes = sortedNotes;
        readingNotes.Tags = readingNotes.GetAllUniqueTags();
        
        return readingNotes;
    }


    private Dictionary<string, List<ReadingNote>> GroupNotesByCategory(List<ReadingNote> notes)
    {
        var sortedNotes = new Dictionary<string, List<ReadingNote>>();

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
                sortedNotes.Add(category, new List<ReadingNote> {note});
            }
        }

        return sortedNotes;
    }

    public bool SaveReadingNotes(ReadingNotes readingNotes)
    {
        var response = httpClient.PostAsJsonAsync("api/notes/SaveReadingNotes", readingNotes);
        
        return response.Result.IsSuccessStatusCode;
    }
}
