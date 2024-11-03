using System;
using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Microsoft.Extensions.Azure;
using NoteBookmark.Domain;

namespace NoteBookmark.Api;

public class DataStorageService(string connectionString):IDataStorageService
{
    private string ConnectionString { get; set; } = connectionString;
    private TableServiceClient? TableClient { get; set; }

    private TableServiceClient GetTableClient()
	{
		if (TableClient == null)
		{
			TableClient = new TableServiceClient(ConnectionString);
		}
		return TableClient;
	}

	private TableClient GetTable(string tableName)
	{
		var client = GetTableClient();
		TableItem table = client.CreateTableIfNotExists(tableName);

        return client.GetTableClient(tableName);
	}

	private TableClient GetPostTable()
    {
        TableClient table = GetTable("Posts");
        return table;
    }

	private TableClient GetSummaryTable()
    {
        TableClient table = GetTable("Summary");
        return table;
    }

    private TableClient GetNoteTable()
    {
        TableClient table = GetTable("Notes");
        return table;
    }

    private TableClient GetSettingTable()
    {
        TableClient table = GetTable("Settings");
        return table;
    }

    public List<ReadingNote> GetNotesForSummary(string ReadingNotesId)
    {
        var tblNotes = GetNoteTable();
        var tblPosts = GetPostTable();

        var notesQuery = tblNotes.Query<Note>(filter: $"PartitionKey eq '{ReadingNotesId}'");
        var notes = notesQuery.ToList();

        var postIds = notes.Select(note => note.PostId).Distinct().ToList();

        var filter = string.Join(" or ", postIds.Select(id => $"RowKey eq '{id}'"));
        var postsQuery = tblPosts.Query<Post>(filter: filter);
        var posts = postsQuery.ToList();


        var result = from note in notes
                        join post in posts on note.PostId equals post.RowKey
                        select new ReadingNote
                        {
                            Comment = note.Comment,
                            Tags = note.Tags,
                            PostId = note.PostId,
                            PostAutor = post.Author,
                            PostTitle = post.Title,
                            PostURL = post.Url,
                            Category = note.Category,
                            PartitionKey = note.PartitionKey,
                            ReadingNotesID = note.PartitionKey,
                            RowKey = note.RowKey
                        };

        return result.ToList();
    }

    //get all notes
    public List<Note> GetNotes()
    {
        var tblNotes = GetNoteTable();
        var notesQuery = tblNotes.Query<Note>();
        var notes = notesQuery.ToList();
        return notes;
    }






    public List<PostL> GetFilteredPosts(string filter)
	{
		var tblPosts = GetPostTable();

        Pageable<Post> posts;
        posts = tblPosts.Query<Post>(filter: filter);
        var tblNotes = GetNoteTable();
        var notes = tblNotes.Query<Note>();

        var joinedResults = from post in posts
                            join note in notes on post.RowKey equals note.PostId into postsnotes
                            from joined in postsnotes.DefaultIfEmpty()
                            orderby post.Timestamp
                            select new PostL
                            { 
                                PartitionKey = post.PartitionKey,
                                RowKey = post.RowKey,
                                Timestamp = post.Timestamp,
                                ETag = post.ETag,
                                Id = post.Id,
                                Date_published = post.Date_published ?? post.Timestamp!.Value.ToString("yyyy-MM-dd" )?? "0000-00-00",
                                is_read = post.is_read ?? false,
                                Title = post.Title ?? string.Empty, 
                                Url = post.Url ?? string.Empty, 
                                Note = joined?.Comment ?? string.Empty, 
                                NoteId = joined?.RowKey ?? string.Empty 
                            };

        List<PostL> lstPosts = joinedResults.ToList();
        return lstPosts;
	}

	public Post? GetPost(string rowKey)
    {
        var tblPost = GetPostTable();
        var result = tblPost.Query<Post>(filter: $"RowKey eq '{rowKey}'");
        Post? post = result.FirstOrDefault<Post>();
        return post;
    }



    public List<Summary> GetSummaries()
    {
        var tblSummary = GetSummaryTable();
        Pageable<Summary> queryResult = tblSummary.Query<Summary>();
        List<Summary> Summaries = queryResult.ToList<Summary>();
        return Summaries;
    }

    public void CreateNote(Note note)
    {
        var tblNote = GetNoteTable();
        var existingNote = tblNote.Query<Note>(filter: $"RowKey eq '{note.RowKey}'").FirstOrDefault();
        if (existingNote != null)
        {
            tblNote.UpdateEntity(note, ETag.All, TableUpdateMode.Replace);
        }
        else
        {
            tblNote.AddEntity<Note>(note);
        }
    }


    public async Task<Settings> GetSettings()
    {
        var tblSettings = GetSettingTable();
        Settings settings;
        var result = await tblSettings.GetEntityIfExistsAsync<Settings>("setting", "setting");
        
        if (result.HasValue)
        {
            settings = result.Value!;
        }
        else
        {
            settings = new Settings {
                PartitionKey = "setting",
                RowKey = "setting",
                LastBookmarkDate = "2023-04-06T07:31:44",
                ReadingNotesCounter = "623"
            };

            await tblSettings.AddEntityAsync<Settings>(settings);
        }
        return settings;
    }

}


