using System.Text.Json;
using ThinkDiary.Core.Models;
using ThinkDiary.Tests.TestData;

namespace ThinkDiary.Tests.UnitTests.Models;

/// <summary>
/// Comprehensive unit tests for the Mood enum
/// </summary>
public class MoodTests
{
    [Fact]
    public void Mood_ShouldHaveExpectedValues()
    {
        // Act
        var moodValues = Enum.GetValues<Mood>();

        // Assert
        moodValues.Should().HaveCount(12, "Mood enum should have exactly 12 values");
        moodValues.Should().Contain(Mood.Happy);
        moodValues.Should().Contain(Mood.Neutral);
        moodValues.Should().Contain(Mood.Sad);
        moodValues.Should().Contain(Mood.Excited);
        moodValues.Should().Contain(Mood.Anxious);
        moodValues.Should().Contain(Mood.Grateful);
        moodValues.Should().Contain(Mood.Reflective);
        moodValues.Should().Contain(Mood.Motivated);
        moodValues.Should().Contain(Mood.Overwhelmed);
        moodValues.Should().Contain(Mood.Peaceful);
        moodValues.Should().Contain(Mood.Frustrated);
        moodValues.Should().Contain(Mood.Hopeful);
    }

    [Theory]
    [InlineData(Mood.Happy, "Happy")]
    [InlineData(Mood.Neutral, "Neutral")]
    [InlineData(Mood.Sad, "Sad")]
    [InlineData(Mood.Excited, "Excited")]
    [InlineData(Mood.Anxious, "Anxious")]
    [InlineData(Mood.Grateful, "Grateful")]
    [InlineData(Mood.Reflective, "Reflective")]
    [InlineData(Mood.Motivated, "Motivated")]
    [InlineData(Mood.Overwhelmed, "Overwhelmed")]
    [InlineData(Mood.Peaceful, "Peaceful")]
    [InlineData(Mood.Frustrated, "Frustrated")]
    [InlineData(Mood.Hopeful, "Hopeful")]
    public void Mood_ToString_ShouldReturnCorrectStringRepresentation(Mood mood, string expectedString)
    {
        // Act
        var result = mood.ToString();

        // Assert
        result.Should().Be(expectedString);
    }

    [Theory]
    [InlineData("Happy", Mood.Happy)]
    [InlineData("Neutral", Mood.Neutral)]
    [InlineData("Sad", Mood.Sad)]
    [InlineData("Excited", Mood.Excited)]
    [InlineData("Anxious", Mood.Anxious)]
    [InlineData("Grateful", Mood.Grateful)]
    [InlineData("Reflective", Mood.Reflective)]
    [InlineData("Motivated", Mood.Motivated)]
    [InlineData("Overwhelmed", Mood.Overwhelmed)]
    [InlineData("Peaceful", Mood.Peaceful)]
    [InlineData("Frustrated", Mood.Frustrated)]
    [InlineData("Hopeful", Mood.Hopeful)]
    public void Mood_Parse_ShouldConvertStringToMoodCorrectly(string moodString, Mood expectedMood)
    {
        // Act
        var result = Enum.Parse<Mood>(moodString);

        // Assert
        result.Should().Be(expectedMood);
    }

    [Theory]
    [InlineData("happy")]
    [InlineData("HAPPY")]
    [InlineData("Happy")]
    public void Mood_Parse_ShouldBeCaseInsensitive(string moodString)
    {
        // Act
        var result = Enum.Parse<Mood>(moodString, ignoreCase: true);

        // Assert
        result.Should().Be(Mood.Happy);
    }

    [Theory]
    [InlineData("InvalidMood")]
    [InlineData("")]
    [InlineData("null")]
    public void Mood_Parse_ShouldThrowForInvalidValues(string invalidMoodString)
    {
        // Act & Assert
        var action = () => Enum.Parse<Mood>(invalidMoodString);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Mood_Parse_ShouldAllowNumericStringsEvenIfNotDefined()
    {
        // This is expected C# enum behavior - numeric strings parse even if not defined
        // Act
        var result = Enum.Parse<Mood>("123");

        // Assert
        result.Should().Be((Mood)123);
        Enum.IsDefined(typeof(Mood), result).Should().BeFalse("123 is not a defined Mood value");
    }

    [Theory]
    [InlineData("Happy", true, true)]
    [InlineData("Sad", true, true)]
    [InlineData("123", true, false)] // Numeric strings parse successfully but may not be defined
    [InlineData("InvalidMood", false, false)]
    [InlineData("", false, false)]
    public void Mood_TryParse_ShouldReturnCorrectResult(string moodString, bool expectedSuccess, bool shouldBeDefined)
    {
        // Act
        var success = Enum.TryParse<Mood>(moodString, out var result);

        // Assert
        success.Should().Be(expectedSuccess);
        if (expectedSuccess)
        {
            if (shouldBeDefined)
            {
                result.Should().BeDefined("result should be a valid defined Mood value");
            }
            else
            {
                // For numeric values that parse but aren't defined
                Enum.IsDefined(typeof(Mood), result).Should().BeFalse();
            }
        }
    }

    [Theory]
    [InlineData(Mood.Happy, 0)]
    [InlineData(Mood.Neutral, 1)]
    [InlineData(Mood.Sad, 2)]
    [InlineData(Mood.Excited, 3)]
    [InlineData(Mood.Anxious, 4)]
    [InlineData(Mood.Grateful, 5)]
    [InlineData(Mood.Reflective, 6)]
    [InlineData(Mood.Motivated, 7)]
    [InlineData(Mood.Overwhelmed, 8)]
    [InlineData(Mood.Peaceful, 9)]
    [InlineData(Mood.Frustrated, 10)]
    [InlineData(Mood.Hopeful, 11)]
    public void Mood_ShouldHaveCorrectUnderlyingValues(Mood mood, int expectedValue)
    {
        // Act
        var intValue = (int)mood;

        // Assert
        intValue.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(0, Mood.Happy)]
    [InlineData(1, Mood.Neutral)]
    [InlineData(2, Mood.Sad)]
    [InlineData(3, Mood.Excited)]
    [InlineData(4, Mood.Anxious)]
    [InlineData(5, Mood.Grateful)]
    [InlineData(6, Mood.Reflective)]
    [InlineData(7, Mood.Motivated)]
    [InlineData(8, Mood.Overwhelmed)]
    [InlineData(9, Mood.Peaceful)]
    [InlineData(10, Mood.Frustrated)]
    [InlineData(11, Mood.Hopeful)]
    public void Mood_ShouldCastFromIntCorrectly(int value, Mood expectedMood)
    {
        // Act
        var mood = (Mood)value;

        // Assert
        mood.Should().Be(expectedMood);
    }

    [Fact]
    public void Mood_JsonSerialization_ShouldWorkCorrectly()
    {
        // Arrange
        var originalMood = Mood.Grateful;

        // Act
        var json = JsonSerializer.Serialize(originalMood);
        var deserializedMood = JsonSerializer.Deserialize<Mood>(json);

        // Assert
        json.Should().NotBeNullOrEmpty();
        deserializedMood.Should().Be(originalMood);
    }

    [Fact]
    public void Mood_JsonSerialization_ShouldSerializeAsNumber()
    {
        // Arrange
        var mood = Mood.Happy; // Should serialize as 0

        // Act
        var json = JsonSerializer.Serialize(mood);

        // Assert
        json.Should().Be("0", "Mood should serialize as its underlying integer value");
    }

    [Fact]
    public void Mood_JsonSerialization_WithStringEnum_ShouldSerializeAsString()
    {
        // Arrange
        var mood = Mood.Peaceful;
        var options = new JsonSerializerOptions
        {
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        // Act
        var json = JsonSerializer.Serialize(mood, options);
        var deserializedMood = JsonSerializer.Deserialize<Mood>(json, options);

        // Assert
        json.Should().Be("\"Peaceful\"", "With JsonStringEnumConverter, Mood should serialize as string");
        deserializedMood.Should().Be(mood);
    }

    [Fact]
    public void Mood_AllValues_ShouldBeAccessible()
    {
        // Act & Assert - Test that all mood values can be accessed and used
        var allMoods = ModelBuilders.MoodBuilder.GetAllMoodValues().ToList();

        allMoods.Should().HaveCount(12);
        allMoods.Should().OnlyHaveUniqueItems();

        // Test each mood can be used in operations
        foreach (var mood in allMoods)
        {
            mood.ToString().Should().NotBeNullOrEmpty();
            ((int)mood).Should().BeInRange(0, 11);
        }
    }

    [Fact]
    public void Mood_IsDefined_ShouldWorkCorrectly()
    {
        // Act & Assert - Valid moods
        Enum.IsDefined(typeof(Mood), Mood.Happy).Should().BeTrue();
        Enum.IsDefined(typeof(Mood), Mood.Hopeful).Should().BeTrue();
        Enum.IsDefined(typeof(Mood), 0).Should().BeTrue(); // Happy
        Enum.IsDefined(typeof(Mood), 11).Should().BeTrue(); // Hopeful

        // Act & Assert - Invalid values
        Enum.IsDefined(typeof(Mood), 12).Should().BeFalse();
        Enum.IsDefined(typeof(Mood), -1).Should().BeFalse();
        Enum.IsDefined(typeof(Mood), 999).Should().BeFalse();
    }

    [Fact]
    public void Mood_GetNames_ShouldReturnAllMoodNames()
    {
        // Act
        var moodNames = Enum.GetNames<Mood>();

        // Assert
        moodNames.Should().HaveCount(12);
        moodNames.Should().Contain("Happy");
        moodNames.Should().Contain("Neutral");
        moodNames.Should().Contain("Sad");
        moodNames.Should().Contain("Excited");
        moodNames.Should().Contain("Anxious");
        moodNames.Should().Contain("Grateful");
        moodNames.Should().Contain("Reflective");
        moodNames.Should().Contain("Motivated");
        moodNames.Should().Contain("Overwhelmed");
        moodNames.Should().Contain("Peaceful");
        moodNames.Should().Contain("Frustrated");
        moodNames.Should().Contain("Hopeful");
    }

    [Fact]
    public void Mood_RandomMood_ShouldReturnValidMood()
    {
        // Act
        var randomMood1 = ModelBuilders.MoodBuilder.GetRandomMood();
        var randomMood2 = ModelBuilders.MoodBuilder.GetRandomMood();

        // Assert
        Enum.IsDefined(typeof(Mood), randomMood1).Should().BeTrue();
        Enum.IsDefined(typeof(Mood), randomMood2).Should().BeTrue();
        
        // Note: We can't assert they're different as random could return same value
        // But we can test multiple times to increase confidence
        var randomMoods = new HashSet<Mood>();
        for (int i = 0; i < 50; i++)
        {
            randomMoods.Add(ModelBuilders.MoodBuilder.GetRandomMood());
        }
        randomMoods.Should().NotBeEmpty();
    }

    [Fact]
    public void Mood_InDiaryEntry_ShouldWorkCorrectly()
    {
        // Arrange
        var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();

        // Act & Assert - Setting mood
        entry.Mood = Mood.Reflective;
        entry.Mood.Should().Be(Mood.Reflective);

        // Act & Assert - Changing mood
        entry.Mood = Mood.Excited;
        entry.Mood.Should().Be(Mood.Excited);

        // Act & Assert - Setting to null
        entry.Mood = null;
        entry.Mood.Should().BeNull();
    }
}