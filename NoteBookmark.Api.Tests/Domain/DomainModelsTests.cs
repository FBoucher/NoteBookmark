using FluentAssertions;
using NoteBookmark.Domain;
using Xunit;

namespace NoteBookmark.Api.Tests.Domain;

public class PostTests
{    [Fact]
    public void Post_WhenCreated_HasCorrectDefaultValues()
    {
        // Act
        var post = new Post 
        { 
            PartitionKey = "posts", 
            RowKey = "test-post" 
        };

        // Assert
        post.is_read.Should().BeNull();
        post.Title.Should().BeNull();
        post.Url.Should().BeNull();
        post.Author.Should().BeNull();
        post.Date_published.Should().BeNull();
        post.Id.Should().BeNull();
    }

    [Fact]
    public void Post_WhenPropertiesSet_ReturnsCorrectValues()
    {
        // Arrange
        var post = new Post
        {
            PartitionKey = "posts",
            RowKey = "test-post-123",
            Title = "Azure Functions Best Practices",
            Url = "https://docs.microsoft.com/azure/functions",
            Author = "Microsoft",
            Date_published = "2025-06-03",
            is_read = true,
            Id = "func-123"
        };

        // Assert
        post.PartitionKey.Should().Be("posts");
        post.RowKey.Should().Be("test-post-123");
        post.Title.Should().Be("Azure Functions Best Practices");
        post.Url.Should().Be("https://docs.microsoft.com/azure/functions");
        post.Author.Should().Be("Microsoft");
        post.Date_published.Should().Be("2025-06-03");
        post.is_read.Should().BeTrue();
        post.Id.Should().Be("func-123");
    }    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public void Post_is_read_CanBeSetToAnyBooleanValue(bool? readStatus)
    {
        // Arrange
        var post = new Post 
        { 
            PartitionKey = "posts", 
            RowKey = "test-post" 
        };

        // Act
        post.is_read = readStatus;

        // Assert
        post.is_read.Should().Be(readStatus);
    }
}

public class NoteTests
{
    [Fact]
    public void Note_WhenCreated_HasCorrectDefaultValues()
    {
        // Act
        var note = new Note();

        // Assert
        note.PostId.Should().BeNull();
        note.Comment.Should().BeNull();
        note.Tags.Should().BeNull();
        note.Category.Should().BeNull();
    }

    [Fact]
    public void Note_WhenPropertiesSet_ReturnsCorrectValues()
    {
        // Arrange
        var note = new Note
        {
            PartitionKey = "reading-notes-123",
            RowKey = "note-456",
            PostId = "post-789",
            Comment = "Excellent article about Azure Functions",
            Tags = "azure, functions, serverless",
            Category = "Technology"
        };

        // Assert
        note.PartitionKey.Should().Be("reading-notes-123");
        note.RowKey.Should().Be("note-456");
        note.PostId.Should().Be("post-789");
        note.Comment.Should().Be("Excellent article about Azure Functions");
        note.Tags.Should().Be("azure, functions, serverless");
        note.Category.Should().Be("Technology");
    }
}

public class SummaryTests
{    [Fact]
    public void Summary_WhenCreated_HasCorrectDefaultValues()
    {
        // Act
        var summary = new Summary 
        { 
            PartitionKey = "summaries", 
            RowKey = "test-summary" 
        };

        // Assert
        summary.Id.Should().BeNull();
        summary.Title.Should().BeNull();
        summary.FileName.Should().BeNull();
    }

    [Fact]
    public void Summary_WhenPropertiesSet_ReturnsCorrectValues()
    {
        // Arrange
        var createdDate = DateTimeOffset.UtcNow;
        var summary = new Summary
        {
            PartitionKey = "summaries",
            RowKey = "summary-123",
            Id = "456",
            Title = "This is a comprehensive summary of reading notes 456.",
            Timestamp = createdDate
        };

        // Assert
        summary.PartitionKey.Should().Be("summaries");
        summary.RowKey.Should().Be("summary-123");
        summary.Id.Should().Be("456");
        summary.Title.Should().Be("This is a comprehensive summary of reading notes 456.");
        summary.Timestamp.Should().Be(createdDate);
    }
}

public class SettingsTests
{    [Fact]
    public void Settings_WhenCreated_HasCorrectDefaultValues()
    {
        // Act
        var settings = new Settings 
        { 
            PartitionKey = "setting", 
            RowKey = "setting" 
        };

        // Assert
        settings.LastBookmarkDate.Should().BeNull();
        settings.ReadingNotesCounter.Should().BeNull();
    }

    [Fact]
    public void Settings_WhenPropertiesSet_ReturnsCorrectValues()
    {
        // Arrange
        var settings = new Settings
        {
            PartitionKey = "setting",
            RowKey = "setting",
            LastBookmarkDate = "2025-06-03T15:30:00",
            ReadingNotesCounter = "750"
        };

        // Assert
        settings.PartitionKey.Should().Be("setting");
        settings.RowKey.Should().Be("setting");
        settings.LastBookmarkDate.Should().Be("2025-06-03T15:30:00");
        settings.ReadingNotesCounter.Should().Be("750");
    }    [Theory]
    [InlineData("2025-06-03T15:30:00")]
    [InlineData("2023-12-25T00:00:00")]
    [InlineData("2024-01-01T12:00:00Z")]
    public void Settings_LastBookmarkDate_CanStoreVariousDateFormats(string dateString)
    {
        // Arrange
        var settings = new Settings 
        { 
            PartitionKey = "setting", 
            RowKey = "setting" 
        };

        // Act
        settings.LastBookmarkDate = dateString;

        // Assert
        settings.LastBookmarkDate.Should().Be(dateString);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("999")]
    [InlineData("1000")]
    public void Settings_ReadingNotesCounter_CanStoreVariousCounterValues(string counter)
    {
        // Arrange
        var settings = new Settings 
        { 
            PartitionKey = "setting", 
            RowKey = "setting" 
        };

        // Act
        settings.ReadingNotesCounter = counter;

        // Assert
        settings.ReadingNotesCounter.Should().Be(counter);
    }
}

public class ReadingNoteTests
{
    [Fact]
    public void ReadingNote_WhenCreated_HasCorrectDefaultValues()
    {
        // Act
        var readingNote = new ReadingNote();

        // Assert
        readingNote.PostId.Should().BeNull();
        readingNote.Title.Should().BeNull();
        readingNote.Url.Should().BeNull();
        readingNote.Author.Should().BeNull();
        readingNote.Comment.Should().BeNull();
        readingNote.Tags.Should().BeNull();
        readingNote.Category.Should().BeNull();
        readingNote.ReadingNotesID.Should().BeNull();
    }

    [Fact]
    public void ReadingNote_WhenPropertiesSet_ReturnsCorrectValues()
    {
        // Arrange
        var readingNote = new ReadingNote
        {
            PartitionKey = "reading-notes-123",
            RowKey = "note-456",
            PostId = "post-789",
            Title = "Azure Functions Performance Tips",
            Url = "https://docs.microsoft.com/azure/functions/performance",
            Author = "Azure Team",
            Comment = "Great insights on optimizing Azure Functions",
            Tags = "azure, functions, performance",
            Category = "Performance",
            ReadingNotesID = "reading-notes-123"
        };

        // Assert
        readingNote.PartitionKey.Should().Be("reading-notes-123");
        readingNote.RowKey.Should().Be("note-456");
        readingNote.PostId.Should().Be("post-789");
        readingNote.Title.Should().Be("Azure Functions Performance Tips");
        readingNote.Url.Should().Be("https://docs.microsoft.com/azure/functions/performance");
        readingNote.Author.Should().Be("Azure Team");
        readingNote.Comment.Should().Be("Great insights on optimizing Azure Functions");
        readingNote.Tags.Should().Be("azure, functions, performance");
        readingNote.Category.Should().Be("Performance");
        readingNote.ReadingNotesID.Should().Be("reading-notes-123");
    }
}

public class ReadingNotesTests
{
    [Fact]
    public void ReadingNotes_WhenCreated_HasCorrectDefaultValues()
    {
        // Act
        var readingNotes = new ReadingNotes("123");

        // Assert
        readingNotes.Number.Should().Be("123");
        readingNotes.Title.Should().Be("Reading Notes #123");
        readingNotes.Notes.Should().NotBeNull();
        readingNotes.Notes.Should().BeEmpty();
    }

    [Fact]
    public void ReadingNotes_WhenPropertiesSet_ReturnsCorrectValues()
    {
        // Arrange
        var readingNotes = new ReadingNotes("456")
        {
            Title = "Custom Reading Notes Title",
            PublishedUrl = "https://example.com/reading-notes-456",
            Tags = "azure, cloud, technology",
            Intro = "This is an introduction to reading notes 456"
        };

        // Assert
        readingNotes.Number.Should().Be("456");
        readingNotes.Title.Should().Be("Custom Reading Notes Title");
        readingNotes.PublishedUrl.Should().Be("https://example.com/reading-notes-456");
        readingNotes.Tags.Should().Be("azure, cloud, technology");
        readingNotes.Intro.Should().Be("This is an introduction to reading notes 456");
    }    [Fact]
    public void ReadingNotes_CanAddNotesToCategories()
    {
        // Arrange
        var readingNotes = new ReadingNotes("789");
        var note1 = new ReadingNote { Title = "Azure Functions", Comment = "Great article" };
        var note2 = new ReadingNote { Title = "Azure Storage", Comment = "Useful tips" };

        // Act
        readingNotes.Notes["Technology"] = new List<ReadingNote> { note1, note2 };

        // Assert
        readingNotes.Notes.Should().ContainKey("Technology");
        readingNotes.Notes["Technology"].Should().HaveCount(2);
        readingNotes.Notes["Technology"].Should().Contain(note1);
        readingNotes.Notes["Technology"].Should().Contain(note2);
    }

    [Fact]
    public void ReadingNotes_GetAllUniqueTags_ReturnsDistinctTags()
    {
        // Arrange
        var readingNotes = new ReadingNotes("100");
        var note1 = new ReadingNote { Tags = "azure.functions.serverless" };
        var note2 = new ReadingNote { Tags = "azure.storage.blob" };
        
        readingNotes.Notes["Technology"] = new List<ReadingNote> { note1, note2 };

        // Act
        var uniqueTags = readingNotes.GetAllUniqueTags();

        // Assert
        uniqueTags.Should().Contain("azure");
        uniqueTags.Should().Contain("functions");
        uniqueTags.Should().Contain("serverless");
        uniqueTags.Should().Contain("storage");
        uniqueTags.Should().Contain("blob");
        uniqueTags.Should().Contain("readingnotes");
    }
}

public class PostLTests
{
    [Fact]
    public void PostL_WhenCreated_HasCorrectDefaultValues()
    {
        // Act
        var postL = new PostL 
        { 
            PartitionKey = "posts", 
            RowKey = "test-post" 
        };

        // Assert
        postL.Id.Should().BeNull();
        postL.Date_published.Should().BeNull();
        postL.is_read.Should().BeNull();
        postL.Title.Should().BeNull();
        postL.Url.Should().BeNull();
        postL.Note.Should().BeNull();
        postL.NoteId.Should().BeNull();
    }

    [Fact]
    public void PostL_WhenPropertiesSet_ReturnsCorrectValues()
    {
        // Arrange
        var postL = new PostL
        {
            PartitionKey = "posts",
            RowKey = "post-123",
            Id = "test-id-123",
            Date_published = "2025-06-03",
            is_read = true,
            Title = "Azure Storage Best Practices",
            Url = "https://docs.microsoft.com/azure/storage",
            Note = "Excellent article with practical examples",
            NoteId = "note-456"
        };

        // Assert
        postL.PartitionKey.Should().Be("posts");
        postL.RowKey.Should().Be("post-123");
        postL.Id.Should().Be("test-id-123");
        postL.Date_published.Should().Be("2025-06-03");
        postL.is_read.Should().BeTrue();
        postL.Title.Should().Be("Azure Storage Best Practices");
        postL.Url.Should().Be("https://docs.microsoft.com/azure/storage");
        postL.Note.Should().Be("Excellent article with practical examples");
        postL.NoteId.Should().Be("note-456");
    }    [Fact]
    public void PostL_Note_CanBeEmptyString()
    {
        // Arrange
        var postL = new PostL
        {
            PartitionKey = "posts",
            RowKey = "test-post",
            Note = ""
        };

        // Assert
        postL.Note.Should().Be("");
    }

    [Fact]
    public void PostL_NoteId_CanBeEmptyString()
    {
        // Arrange
        var postL = new PostL
        {
            PartitionKey = "posts",
            RowKey = "test-post",
            NoteId = ""
        };

        // Assert
        postL.NoteId.Should().Be("");
    }
}
