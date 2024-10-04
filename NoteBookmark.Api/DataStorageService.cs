using System;
using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;

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

	public List<Post> GetFilteredPosts(string filter)
	{
		var tblPosts = GetPostTable();

        Pageable<Post> posts;
        posts = tblPosts.Query<Post>(filter:filter);
        
        List<Post> lstPosts = posts.ToList<Post>();
        return lstPosts;
	}

	public Post? GetPost(string rowKey)
    {
        var tblPost = GetPostTable();
        var result = tblPost.Query<Post>(filter: $"RowKey eq '{rowKey}'");
        Post? post = result.FirstOrDefault<Post>();
        return post;
    }

}
