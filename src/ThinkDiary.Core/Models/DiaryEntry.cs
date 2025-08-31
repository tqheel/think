namespace ThinkDiary.Core.Models;

public class DiaryEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; // Markdown text
    public List<Tag> Tags { get; set; } = new();
    public Mood? Mood { get; set; }
    public int WordCount { get; set; }
    
    // AI-generated metadata
    public string? Summary { get; set; }
    public List<string>? Themes { get; set; }
    public double? SentimentScore { get; set; }
}
