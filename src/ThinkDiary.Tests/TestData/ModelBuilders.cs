using ThinkDiary.Core.Models;

namespace ThinkDiary.Tests.TestData;

/// <summary>
/// Test data builders for creating consistent test data across all test classes
/// </summary>
public static class ModelBuilders
{
    public static class DiaryEntryBuilder
    {
        public static DiaryEntry CreateDefault()
        {
            return new DiaryEntry
            {
                Title = "Test Entry",
                Content = "This is a test diary entry content.",
                WordCount = 8
            };
        }

        public static DiaryEntry CreateWithTags(params Tag[] tags)
        {
            var entry = CreateDefault();
            entry.Tags = tags.ToList();
            return entry;
        }

        public static DiaryEntry CreateWithMood(Mood mood)
        {
            var entry = CreateDefault();
            entry.Mood = mood;
            return entry;
        }

        public static DiaryEntry CreateWithAIMetadata()
        {
            var entry = CreateDefault();
            entry.Summary = "Test summary";
            entry.Themes = new List<string> { "personal", "reflection" };
            entry.SentimentScore = 0.8;
            return entry;
        }

        public static DiaryEntry CreateWithSpecificId(Guid id)
        {
            var entry = CreateDefault();
            entry.Id = id;
            return entry;
        }

        public static DiaryEntry CreateWithSpecificDates(DateTime createdAt, DateTime updatedAt)
        {
            var entry = CreateDefault();
            entry.CreatedAt = createdAt;
            entry.UpdatedAt = updatedAt;
            return entry;
        }

        public static DiaryEntry CreateEmpty()
        {
            return new DiaryEntry();
        }
    }

    public static class TagBuilder
    {
        public static Tag CreateDefault()
        {
            return new Tag
            {
                Id = 1,
                Name = "Test Tag",
                Color = "#FF0000"
            };
        }

        public static Tag CreateWithName(string name)
        {
            var tag = CreateDefault();
            tag.Name = name;
            return tag;
        }

        public static Tag CreateWithColor(string color)
        {
            var tag = CreateDefault();
            tag.Color = color;
            return tag;
        }

        public static Tag CreateWithoutColor()
        {
            var tag = CreateDefault();
            tag.Color = null;
            return tag;
        }

        public static Tag CreateWithEntries(params DiaryEntry[] entries)
        {
            var tag = CreateDefault();
            tag.Entries = entries.ToList();
            return tag;
        }

        public static Tag CreateEmpty()
        {
            return new Tag();
        }
    }

    public static class MoodBuilder
    {
        public static IEnumerable<Mood> GetAllMoodValues()
        {
            return Enum.GetValues<Mood>();
        }

        public static Mood GetRandomMood()
        {
            var moods = GetAllMoodValues().ToArray();
            var random = Random.Shared;
            return moods[random.Next(moods.Length)];
        }
    }
}