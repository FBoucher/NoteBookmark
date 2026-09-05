using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Moq;
using NoteBookmark.BlazorApp.Tests.Helpers;
using NoteBookmark.Domain;
using NoteBookmark.SharedUI;
using NoteBookmark.SharedUI.Components.Pages;
using Xunit;

namespace NoteBookmark.BlazorApp.Tests.Tests;

public sealed class PostReaderTests : BunitContext
{
    private readonly Mock<IDataService> _dataServiceMock;

    public PostReaderTests()
    {
        this.AddFluentUI();
        this.AddAuthorization().SetAuthorized("testuser");

        _dataServiceMock = new Mock<IDataService>();
        _dataServiceMock.Setup(s => s.GetPost("p1")).ReturnsAsync(new Post
        {
            PartitionKey = "p",
            RowKey = "p1",
            Title = "Test Offline Article Title",
            Author = "Frank Boucher",
            Date_published = "2026-01-01T00:00:00"
        });
        _dataServiceMock.Setup(s => s.GetPostHtmlAsync("p1")).ReturnsAsync("<p>Hello offline reader world</p>");

        Services.AddSingleton(_dataServiceMock.Object);
    }

    [Fact]
    public void PostReader_RendersTitleAndContentAndSlidersAndBackButtonsAtTopAndBottom()
    {
        var cut = Render<PostReader>(ps => ps.Add(p => p.PostId, "p1"));

        cut.Markup.Should().Contain("Test Offline Article Title");
        cut.Markup.Should().Contain("Frank Boucher");
        cut.Markup.Should().Contain("Hello offline reader world");
        cut.Markup.Should().Contain("reader-content");
        cut.Markup.Should().Contain("Text size:");

        var sliders = cut.FindComponents<FluentSlider<int>>();
        sliders.Should().HaveCount(2);
        sliders[0].Instance.Min.Should().Be(8);
        sliders[0].Instance.Max.Should().Be(56);
        sliders[1].Instance.Min.Should().Be(8);
        sliders[1].Instance.Max.Should().Be(56);

        var backButtons = cut.FindComponents<FluentButton>()
            .Where(b => b.Instance.Title == "Back to posts")
            .ToList();
        backButtons.Should().HaveCount(2);
    }

    [Fact]
    public void PostReader_TopSliderValueChange_UpdatesContentFontSize()
    {
        var cut = Render<PostReader>(ps => ps.Add(p => p.PostId, "p1"));

        var contentDivBefore = cut.Find("div.reader-content");
        contentDivBefore.GetAttribute("style").Should().Contain("font-size: 16px;");

        var sliders = cut.FindComponents<FluentSlider<int>>();
        cut.InvokeAsync(() => sliders[0].Instance.ValueChanged.InvokeAsync(24));

        var contentDivAfter = cut.Find("div.reader-content");
        contentDivAfter.GetAttribute("style").Should().Contain("font-size: 24px;");
    }

    [Fact]
    public void PostReader_BottomSliderValueChange_UpdatesContentFontSize()
    {
        var cut = Render<PostReader>(ps => ps.Add(p => p.PostId, "p1"));

        var sliders = cut.FindComponents<FluentSlider<int>>();
        cut.InvokeAsync(() => sliders[1].Instance.ValueChanged.InvokeAsync(20));

        var contentDivAfter = cut.Find("div.reader-content");
        contentDivAfter.GetAttribute("style").Should().Contain("font-size: 20px;");
    }
}
