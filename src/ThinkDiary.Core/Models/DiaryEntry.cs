using Google.Cloud.Firestore;

namespace ThinkDiary.Core.Models;

[FirestoreData]
public class DiaryEntry
{
    [FirestoreProperty]
    public Guid Id { get; set; } = Guid.NewGuid();
    [FirestoreProperty]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [FirestoreProperty]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    [FirestoreProperty]
    public string Title { get; set; } = string.Empty;
    [FirestoreProperty]
    public string Content { get; set; } = string.Empty; // Markdown text
    [FirestoreProperty]
    public List<string> TagIds { get; set; } = new(); // Store tag IDs for many-to-many relationship
    [FirestoreProperty]
    public Mood? Mood { get; set; }
    [FirestoreProperty]
    public int WordCount { get; set; }
    
    // AI-generated metadata
    [FirestoreProperty]
    public string? Summary { get; set; }
    [FirestoreProperty]
    public List<string>? Themes { get; set; }
    [FirestoreProperty]
    public double? SentimentScore { get; set; }

    // Navigation property - not stored in Firestore
    public List<Tag> Tags { get; set; } = new();
}
