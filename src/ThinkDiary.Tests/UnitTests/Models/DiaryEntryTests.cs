using ThinkDiary.Core.Models;
using ThinkDiary.Tests.TestData;

namespace ThinkDiary.Tests.UnitTests.Models;

/// <summary>
/// Comprehensive unit tests for the DiaryEntry model
/// </summary>
public class DiaryEntryTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var entry = new DiaryEntry();

        // Assert
        entry.Id.Should().NotBeEmpty("Id should be automatically generated");
        entry.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1), "CreatedAt should be set to current UTC time");
        entry.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1), "UpdatedAt should be set to current UTC time");
        entry.Title.Should().Be(string.Empty, "Title should default to empty string");
        entry.Content.Should().Be(string.Empty, "Content should default to empty string");
        entry.Tags.Should().NotBeNull().And.BeEmpty("Tags should be initialized as empty list");
        entry.Mood.Should().BeNull("Mood should be nullable and default to null");
        entry.WordCount.Should().Be(0, "WordCount should default to 0");
        entry.Summary.Should().BeNull("Summary should be nullable and default to null");
        entry.Themes.Should().BeNull("Themes should be nullable and default to null");
        entry.SentimentScore.Should().BeNull("SentimentScore should be nullable and default to null");
    }

    [Fact]
    public void Id_ShouldBeUniqueForEachInstance()
    {
        // Act
        var entry1 = new DiaryEntry();
        var entry2 = new DiaryEntry();

        // Assert
        entry1.Id.Should().NotBe(entry2.Id, "Each DiaryEntry should have a unique Id");
        entry1.Id.Should().NotBeEmpty();
        entry2.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Id_SetAndGet_ShouldWorkCorrectly()
    {
        // Arrange
        var entry = new DiaryEntry();
        var newId = Guid.NewGuid();

        // Act
        entry.Id = newId;

        // Assert
        entry.Id.Should().Be(newId);
    }

    [Theory]
    [InlineData("Sample Title")]
    [InlineData("")]
    [InlineData("Very Long Title That Exceeds Normal Expectations For A Diary Entry Title But Should Still Be Supported")]
    public void Title_SetAndGet_ShouldWorkCorrectly(string title)
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();

        // Act
        entry.Title = title;

        // Assert
        entry.Title.Should().Be(title);
    }

    [Theory]
    [InlineData("Sample content")]
    [InlineData("")]
    [InlineData("# Markdown Content\n\nThis is **bold** text.\n\n- List item 1\n- List item 2")]
    public void Content_SetAndGet_ShouldWorkCorrectly(string content)
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();

        // Act
        entry.Content = content;

        // Assert
        entry.Content.Should().Be(content);
    }

    [Fact]
    public void CreatedAt_SetAndGet_ShouldWorkCorrectly()
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();
        var specificDate = new DateTime(2023, 12, 25, 10, 30, 0, DateTimeKind.Utc);

        // Act
        entry.CreatedAt = specificDate;

        // Assert
        entry.CreatedAt.Should().Be(specificDate);
    }

    [Fact]
    public void UpdatedAt_SetAndGet_ShouldWorkCorrectly()
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();
        var specificDate = new DateTime(2023, 12, 26, 15, 45, 0, DateTimeKind.Utc);

        // Act
        entry.UpdatedAt = specificDate;

        // Assert
        entry.UpdatedAt.Should().Be(specificDate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(10000)]
    public void WordCount_SetAndGet_ShouldWorkCorrectly(int wordCount)
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();

        // Act
        entry.WordCount = wordCount;

        // Assert
        entry.WordCount.Should().Be(wordCount);
    }

    [Fact]
    public void Tags_ShouldBeInitializedAsEmptyList()
    {
        // Act
        var entry = new DiaryEntry();

        // Assert
        entry.Tags.Should().NotBeNull();
        entry.Tags.Should().BeEmpty();
        entry.Tags.Should().BeOfType<List<Tag>>();
    }

    [Fact]
    public void Tags_ShouldAllowAddingAndRemovingTags()
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();
        var tag1 = ModelBuilders.TagBuilder.CreateWithName("Work");
        var tag2 = ModelBuilders.TagBuilder.CreateWithName("Personal");

        // Act
        entry.Tags.Add(tag1);
        entry.Tags.Add(tag2);

        // Assert
        entry.Tags.Should().HaveCount(2);
        entry.Tags.Should().Contain(tag1);
        entry.Tags.Should().Contain(tag2);

        // Act - Remove
        entry.Tags.Remove(tag1);

        // Assert
        entry.Tags.Should().HaveCount(1);
        entry.Tags.Should().Contain(tag2);
        entry.Tags.Should().NotContain(tag1);
    }

    [Theory]
    [InlineData(Mood.Happy)]
    [InlineData(Mood.Sad)]
    [InlineData(Mood.Neutral)]
    [InlineData(Mood.Excited)]
    [InlineData(Mood.Anxious)]
    [InlineData(Mood.Grateful)]
    [InlineData(Mood.Reflective)]
    [InlineData(Mood.Motivated)]
    [InlineData(Mood.Overwhelmed)]
    [InlineData(Mood.Peaceful)]
    [InlineData(Mood.Frustrated)]
    [InlineData(Mood.Hopeful)]
    public void Mood_ShouldAcceptAllValidMoodValues(Mood mood)
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();

        // Act
        entry.Mood = mood;

        // Assert
        entry.Mood.Should().Be(mood);
    }

    [Fact]
    public void Mood_ShouldAcceptNullValue()
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateWithMood(Mood.Happy);

        // Act
        entry.Mood = null;

        // Assert
        entry.Mood.Should().BeNull();
    }

    [Theory]
    [InlineData("Short summary")]
    [InlineData("")]
    [InlineData(null)]
    public void Summary_SetAndGet_ShouldWorkCorrectly(string? summary)
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();

        // Act
        entry.Summary = summary;

        // Assert
        entry.Summary.Should().Be(summary);
    }

    [Fact]
    public void Themes_ShouldAcceptNullValue()
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateWithAIMetadata();

        // Act
        entry.Themes = null;

        // Assert
        entry.Themes.Should().BeNull();
    }

    [Fact]
    public void Themes_ShouldAcceptListOfStrings()
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();
        var themes = new List<string> { "work", "family", "health" };

        // Act
        entry.Themes = themes;

        // Assert
        entry.Themes.Should().NotBeNull();
        entry.Themes.Should().HaveCount(3);
        entry.Themes.Should().Contain("work");
        entry.Themes.Should().Contain("family");
        entry.Themes.Should().Contain("health");
    }

    [Fact]
    public void Themes_ShouldAcceptEmptyList()
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();
        var themes = new List<string>();

        // Act
        entry.Themes = themes;

        // Assert
        entry.Themes.Should().NotBeNull();
        entry.Themes.Should().BeEmpty();
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(0.0)]
    [InlineData(0.5)]
    [InlineData(1.0)]
    [InlineData(null)]
    public void SentimentScore_SetAndGet_ShouldWorkCorrectly(double? sentimentScore)
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();

        // Act
        entry.SentimentScore = sentimentScore;

        // Assert
        entry.SentimentScore.Should().Be(sentimentScore);
    }

    [Fact]
    public void CreatedAtAndUpdatedAt_ShouldBeIndependentProperties()
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();
        var createdDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var updatedDate = new DateTime(2023, 6, 15, 12, 0, 0, DateTimeKind.Utc);

        // Act
        entry.CreatedAt = createdDate;
        entry.UpdatedAt = updatedDate;

        // Assert
        entry.CreatedAt.Should().Be(createdDate);
        entry.UpdatedAt.Should().Be(updatedDate);
        entry.CreatedAt.Should().NotBe(entry.UpdatedAt);
    }

    [Fact]
    public void AllProperties_ShouldBeIndependentlySettable()
    {
        // Arrange
        var entry = new DiaryEntry();
        var testId = Guid.NewGuid();
        var testDate = new DateTime(2023, 5, 10, 14, 30, 0, DateTimeKind.Utc);
        var testTag = ModelBuilders.TagBuilder.CreateWithName("Test");
        var testThemes = new List<string> { "theme1", "theme2" };

        // Act
        entry.Id = testId;
        entry.CreatedAt = testDate;
        entry.UpdatedAt = testDate.AddHours(1);
        entry.Title = "Test Title";
        entry.Content = "Test Content";
        entry.Tags.Add(testTag);
        entry.Mood = Mood.Happy;
        entry.WordCount = 42;
        entry.Summary = "Test Summary";
        entry.Themes = testThemes;
        entry.SentimentScore = 0.75;

        // Assert
        entry.Id.Should().Be(testId);
        entry.CreatedAt.Should().Be(testDate);
        entry.UpdatedAt.Should().Be(testDate.AddHours(1));
        entry.Title.Should().Be("Test Title");
        entry.Content.Should().Be("Test Content");
        entry.Tags.Should().Contain(testTag);
        entry.Mood.Should().Be(Mood.Happy);
        entry.WordCount.Should().Be(42);
        entry.Summary.Should().Be("Test Summary");
        entry.Themes.Should().Equal(testThemes);
        entry.SentimentScore.Should().Be(0.75);
    }
}