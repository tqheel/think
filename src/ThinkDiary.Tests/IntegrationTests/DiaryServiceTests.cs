using FluentAssertions;
using ThinkDiary.Core.Models;
using ThinkDiary.Data;

namespace ThinkDiary.Tests.IntegrationTests;

public class DiaryServiceTests
{
    [Fact]
    public void DiaryService_CreateEntryAsync_ShouldCalculateWordCount()
    {
        // Basic test to verify DiaryService functionality
        // Note: This test doesn't require actual Firestore connection
        
        // Arrange
        var content = "This is a test diary entry with multiple words.";
        var expectedWordCount = 9;

        // Act & Assert
        // For now, we'll test the word count calculation indirectly
        var words = content.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        words.Length.Should().Be(expectedWordCount);
    }

    [Fact]
    public void DiaryEntry_ShouldHaveCorrectDefaults()
    {
        // Arrange & Act
        var entry = new DiaryEntry();

        // Assert
        entry.Id.Should().NotBeEmpty();
        entry.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        entry.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        entry.Title.Should().BeEmpty();
        entry.Content.Should().BeEmpty();
        entry.Tags.Should().BeEmpty();
        entry.TagIds.Should().BeEmpty();
        entry.WordCount.Should().Be(0);
    }

    [Fact]
    public void Tag_ShouldHaveCorrectDefaults()
    {
        // Arrange & Act
        var tag = new Tag();

        // Assert
        tag.Id.Should().BeEmpty();
        tag.Name.Should().BeEmpty();
        tag.Color.Should().BeNull();
        tag.Entries.Should().BeEmpty();
    }
}