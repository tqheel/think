using ThinkDiary.Core.Models;
using ThinkDiary.Tests.TestData;

namespace ThinkDiary.Tests.UnitTests.Models;

/// <summary>
/// Comprehensive unit tests for the Tag model
/// </summary>
public class TagTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var tag = new Tag();

        // Assert
        tag.Id.Should().Be(0, "Id should default to 0");
        tag.Name.Should().Be(string.Empty, "Name should default to empty string");
        tag.Color.Should().BeNull("Color should be nullable and default to null");
        tag.Entries.Should().NotBeNull().And.BeEmpty("Entries should be initialized as empty list");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Id_SetAndGet_ShouldWorkCorrectly(int id)
    {
        // Arrange
        var tag = ModelBuilders.TagBuilder.CreateDefault();

        // Act
        tag.Id = id;

        // Assert
        tag.Id.Should().Be(id);
    }

    [Theory]
    [InlineData("Work")]
    [InlineData("Personal")]
    [InlineData("")]
    [InlineData("Very Long Tag Name That Exceeds Normal Expectations")]
    [InlineData("Tag with Special Characters: !@#$%^&*()")]
    [InlineData("Tag with Unicode: 🏷️📝✨")]
    public void Name_SetAndGet_ShouldWorkCorrectly(string name)
    {
        // Arrange
        var tag = ModelBuilders.TagBuilder.CreateDefault();

        // Act
        tag.Name = name;

        // Assert
        tag.Name.Should().Be(name);
    }

    [Theory]
    [InlineData("#FF0000")] // Red
    [InlineData("#00FF00")] // Green
    [InlineData("#0000FF")] // Blue
    [InlineData("#FFFFFF")] // White
    [InlineData("#000000")] // Black
    [InlineData("#ff0000")] // Lowercase hex
    [InlineData("")] // Empty string
    [InlineData(null)] // Null value
    public void Color_SetAndGet_ShouldWorkCorrectly(string? color)
    {
        // Arrange
        var tag = ModelBuilders.TagBuilder.CreateDefault();

        // Act
        tag.Color = color;

        // Assert
        tag.Color.Should().Be(color);
    }

    [Fact]
    public void Color_ShouldAcceptNullValue()
    {
        // Arrange
        var tag = ModelBuilders.TagBuilder.CreateWithColor("#FF0000");

        // Act
        tag.Color = null;

        // Assert
        tag.Color.Should().BeNull();
    }

    [Fact]
    public void Entries_ShouldBeInitializedAsEmptyList()
    {
        // Act
        var tag = new Tag();

        // Assert
        tag.Entries.Should().NotBeNull();
        tag.Entries.Should().BeEmpty();
        tag.Entries.Should().BeOfType<List<DiaryEntry>>();
    }

    [Fact]
    public void Entries_ShouldAllowAddingAndRemovingEntries()
    {
        // Arrange
        var tag = ModelBuilders.TagBuilder.CreateDefault();
        var entry1 = ModelBuilders.DiaryEntryBuilder.CreateDefault();
        var entry2 = ModelBuilders.DiaryEntryBuilder.CreateDefault();

        // Act
        tag.Entries.Add(entry1);
        tag.Entries.Add(entry2);

        // Assert
        tag.Entries.Should().HaveCount(2);
        tag.Entries.Should().Contain(entry1);
        tag.Entries.Should().Contain(entry2);

        // Act - Remove
        tag.Entries.Remove(entry1);

        // Assert
        tag.Entries.Should().HaveCount(1);
        tag.Entries.Should().Contain(entry2);
        tag.Entries.Should().NotContain(entry1);
    }

    [Fact]
    public void Entries_ShouldAllowMultipleEntriesWithSameContent()
    {
        // Arrange
        var tag = ModelBuilders.TagBuilder.CreateDefault();
        var entry1 = ModelBuilders.DiaryEntryBuilder.CreateDefault();
        var entry2 = ModelBuilders.DiaryEntryBuilder.CreateDefault();
        // Both entries have same content but different IDs

        // Act
        tag.Entries.Add(entry1);
        tag.Entries.Add(entry2);

        // Assert
        tag.Entries.Should().HaveCount(2);
        tag.Entries.Should().Contain(entry1);
        tag.Entries.Should().Contain(entry2);
        tag.Entries[0].Id.Should().NotBe(tag.Entries[1].Id, "Entries should have different IDs even with same content");
    }

    [Fact]
    public void AllProperties_ShouldBeIndependentlySettable()
    {
        // Arrange
        var tag = new Tag();
        var testEntry = ModelBuilders.DiaryEntryBuilder.CreateDefault();

        // Act
        tag.Id = 42;
        tag.Name = "Test Tag";
        tag.Color = "#FF5733";
        tag.Entries.Add(testEntry);

        // Assert
        tag.Id.Should().Be(42);
        tag.Name.Should().Be("Test Tag");
        tag.Color.Should().Be("#FF5733");
        tag.Entries.Should().HaveCount(1);
        tag.Entries.Should().Contain(testEntry);
    }

    [Fact]
    public void Tag_WithoutColor_ShouldHaveNullColor()
    {
        // Act
        var tag = ModelBuilders.TagBuilder.CreateWithoutColor();

        // Assert
        tag.Color.Should().BeNull();
        tag.Name.Should().NotBeNullOrEmpty();
        tag.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Tag_WithColor_ShouldRetainColorValue()
    {
        // Arrange
        const string expectedColor = "#00BFFF";

        // Act
        var tag = ModelBuilders.TagBuilder.CreateWithColor(expectedColor);

        // Assert
        tag.Color.Should().Be(expectedColor);
    }

    [Fact]
    public void Tag_WithEntries_ShouldContainAllProvidedEntries()
    {
        // Arrange
        var entry1 = ModelBuilders.DiaryEntryBuilder.CreateDefault();
        var entry2 = ModelBuilders.DiaryEntryBuilder.CreateDefault();
        var entry3 = ModelBuilders.DiaryEntryBuilder.CreateDefault();

        // Act
        var tag = ModelBuilders.TagBuilder.CreateWithEntries(entry1, entry2, entry3);

        // Assert
        tag.Entries.Should().HaveCount(3);
        tag.Entries.Should().Contain(entry1);
        tag.Entries.Should().Contain(entry2);
        tag.Entries.Should().Contain(entry3);
    }

    [Fact]
    public void Tag_EmptyBuilder_ShouldCreateTagWithDefaults()
    {
        // Act
        var tag = ModelBuilders.TagBuilder.CreateEmpty();

        // Assert
        tag.Id.Should().Be(0);
        tag.Name.Should().Be(string.Empty);
        tag.Color.Should().BeNull();
        tag.Entries.Should().BeEmpty();
    }

    [Theory]
    [InlineData("red")]
    [InlineData("blue")]
    [InlineData("work-tag")]
    [InlineData("Personal Journey")]
    public void Tag_WithSpecificName_ShouldRetainNameValue(string name)
    {
        // Act
        var tag = ModelBuilders.TagBuilder.CreateWithName(name);

        // Assert
        tag.Name.Should().Be(name);
        tag.Id.Should().BeGreaterThan(0);
        tag.Entries.Should().BeEmpty();
    }

    [Fact]
    public void Entries_ShouldSupportCollectionOperations()
    {
        // Arrange
        var tag = ModelBuilders.TagBuilder.CreateDefault();
        var entries = new List<DiaryEntry>
        {
            ModelBuilders.DiaryEntryBuilder.CreateDefault(),
            ModelBuilders.DiaryEntryBuilder.CreateDefault(),
            ModelBuilders.DiaryEntryBuilder.CreateDefault()
        };

        // Act & Assert - AddRange equivalent
        foreach (var entry in entries)
        {
            tag.Entries.Add(entry);
        }
        tag.Entries.Should().HaveCount(3);

        // Act & Assert - Clear
        tag.Entries.Clear();
        tag.Entries.Should().BeEmpty();

        // Act & Assert - Single add after clear
        tag.Entries.Add(entries[0]);
        tag.Entries.Should().HaveCount(1);
        tag.Entries.Should().Contain(entries[0]);
    }

    [Fact]
    public void Tag_PropertiesAreIndependent()
    {
        // Arrange
        var tag1 = ModelBuilders.TagBuilder.CreateWithName("Tag1");
        var tag2 = ModelBuilders.TagBuilder.CreateWithName("Tag2");

        // Act
        tag1.Color = "#FF0000";
        tag2.Color = "#00FF00";

        // Assert
        tag1.Name.Should().Be("Tag1");
        tag1.Color.Should().Be("#FF0000");
        tag2.Name.Should().Be("Tag2");
        tag2.Color.Should().Be("#00FF00");
        tag1.Color.Should().NotBe(tag2.Color);
        tag1.Name.Should().NotBe(tag2.Name);
    }
}