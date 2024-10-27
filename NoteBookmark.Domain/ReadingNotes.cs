using System;
using System.Text.Json;


namespace NoteBookmark.Domain;

public class ReadingNotes
{
    public ReadingNotes(int number)
    {
        this.Number = number.ToString();
        this.Title = $"Reading Notes #{number}";
        this.Notes = new Dictionary<string, List<Note>>();
    }

    
    public ReadingNotes(string jsonFilePath)
    {
        var jsonString = File.ReadAllText(jsonFilePath);
        var readingNotes = JsonSerializer.Deserialize<ReadingNotes>(jsonString);

        if (readingNotes != null)
        {
            this.Number = readingNotes.Number;
            this.Title = readingNotes.Title;
            this.Tags = readingNotes.Tags;
            this.Intro = readingNotes.Intro;
            this.Notes = readingNotes.Notes;
        }
    }

    public string Number { get; set; }
    public string  Title { get; set; } = string.Empty;
    //public string  Filename { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Intro { get; set; } = string.Empty;
    public Dictionary<string, List<Note>> Notes { get; set; }
}