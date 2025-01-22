using System;
using Microsoft.AspNetCore.Http.HttpResults;
using NoteBookmark.Domain;
using static NoteBookmark.Api.DataStorageService;
using HtmlAgilityPack;

namespace NoteBookmark.Api;

public static class PostEndpoints
{
	public static void MapPostEndpoints(this IEndpointRouteBuilder app)
	{
		var endpoints = app.MapGroup("api/posts")
				.WithOpenApi();

		endpoints.MapGet("/", GetUnreadPosts)
			.WithDescription("Get all unread posts");
		endpoints.MapGet("/{id}", Get)
			.WithDescription("Get a post by id");
		endpoints.MapPost("/", SavePost)
			.WithDescription("Save or Create a post");
		endpoints.MapPost("/extractPostDetails", ExtractPostDetails)
			.WithDescription("Extract post details from URL and save the post");
	}
	
	static List<PostL> GetUnreadPosts(IDataStorageService dataStorageService)
	{
		return dataStorageService.GetFilteredPosts("is_read eq false");
	}

	static Results<Ok<Post>, NotFound> Get(string id, IDataStorageService dataStorageService)
	{  
		var post = dataStorageService.GetPost(id);
		return post is null ? TypedResults.NotFound() : TypedResults.Ok(post);
	}

	static Results<Ok, BadRequest> SavePost(Post post, IDataStorageService dataStorageService)
	{
		if (dataStorageService.SavePost(post))
		{
			return TypedResults.Ok();
		}
		return TypedResults.BadRequest();
	}

	static async Task<Results<Ok<Post>, BadRequest>> ExtractPostDetails(string url, IDataStorageService dataStorageService)
	{
		try
		{
			var post = await ExtractPostDetailsFromUrl(url);
			if (post != null)
			{
				dataStorageService.SavePost(post);
				return TypedResults.Ok(post);
			}
			return TypedResults.BadRequest();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred while extracting post details: {ex.Message}");
			return TypedResults.BadRequest();
		}
	}


	private static async Task<Post?> ExtractPostDetailsFromUrl(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var titleNode = doc.DocumentNode.SelectSingleNode("//head/title");
        var authorNode = doc.DocumentNode.SelectSingleNode("//meta[@name='author']");
		var descriptionNode = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");

		var publicationDateNode = doc.DocumentNode.SelectSingleNode("//meta[@property='article:published_time']") ??
								  doc.DocumentNode.SelectSingleNode("//meta[@name='pubdate']") ??
								  doc.DocumentNode.SelectSingleNode("//time[@class='entry-date']");

		DateTime publicationDate = DateTime.UtcNow;
		if (publicationDateNode != null)
		{
			var dateContent = publicationDateNode.GetAttributeValue("content", string.Empty) ??
							  publicationDateNode.GetAttributeValue("datetime", string.Empty) ??
							  publicationDateNode.InnerText;

			if (DateTime.TryParse(dateContent, out var parsedDate))
			{
				publicationDate = parsedDate;
			}
		}

		var postGuid = Guid.NewGuid().ToString();
		var post = new Post
		{
			Url = url,
			Domain = new Uri(url).Host,
			Title = titleNode?.InnerText,
			Author = authorNode?.GetAttributeValue("content", string.Empty),
			Excerpt = descriptionNode?.GetAttributeValue("content", string.Empty),
			PartitionKey = DateTime.UtcNow.ToString("yyyy-MM"),
			Date_published = publicationDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
			is_read = false,
			RowKey = postGuid,
			Id = postGuid
		};

		return post;
    }
}
