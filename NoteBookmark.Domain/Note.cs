using System.Runtime.Serialization;
using Azure.Data.Tables;

namespace NoteBookmark.Domain;

public class Note : ITableEntity
{
    public Note()
    {
        PartitionKey = DateTime.UtcNow.ToString("yyyy-MM");
        RowKey = Guid.NewGuid().ToString();
    }


    [DataMember(Name = "comment")]
    public string? Comment { get; set; }

    [DataMember(Name = "date_added")]
    public DateTime DateAdded { get; set; }

    [DataMember(Name = "tags")]
    public string? Tags { get; set; }

    [DataMember(Name = "post_id")]
    public string? PostId { get; set; }

    [DataMember(Name = "category")]
    private string? _category { get; set; }
    public string? Category
    {
        get => string.IsNullOrWhiteSpace(_category) ? GetCategory() : _category;
        set { _category = value; }
    }

    [DataMember(Name = "reading_notes_id")]
    public string? ReadingNotesID { get; set; }



    public string PartitionKey { get; set; }

    public string RowKey { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public Azure.ETag ETag { get; set; }

    public bool Validate()
    {
        return !string.IsNullOrWhiteSpace(Comment);
    }

    private string GetCategory()
    {
        string category = "misc";
        if (!string.IsNullOrEmpty(Tags))
        {
            var newListTags = Tags.Split('.');

            category = newListTags[0];
        }

        return NoteCategories.GetCategories(category);
    }
}
