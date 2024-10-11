using System;
using System.Runtime.Serialization;

namespace NoteBookmark.Domain;

public class Note
{
    public string Id { get; set; }
    public string Comment { get; set; }
    public DateTime DateAdded { get; set; }
    public string Tags { get; set; }
    public string PostId { get; set; }
}
