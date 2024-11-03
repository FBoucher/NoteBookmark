using System.Runtime.Serialization;
using Azure.Data.Tables;

namespace NoteBookmark.Domain;

public class ReadingNote 
{
    public string? Comment { get; set; }
    public string? Tags { get; set; }

    public string? PostId { get; set; }

    public string? PostAutor { get; set; }

    public string? PostTitle { get; set; }
    public string? PostURL { get; set; }

    private string? _category { get; set; }
    public string? Category
    {
        get => string.IsNullOrWhiteSpace(_category) ? GetCategory() : _category;
        set { _category = value; }
    }
    public string? ReadingNotesID { get; set; }

    public string? PartitionKey { get; set; }

    public string? RowKey { get; set; }

    private string GetCategory()
    {
        string category = "misc";
        if (!string.IsNullOrEmpty(Tags))
        {
            var newListTags = Tags.Split('.');

            category = newListTags[0];
        }

        return NoteCategories.GetCategory(category);
    }
}
