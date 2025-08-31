namespace ThinkDiary.Core.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
    public List<DiaryEntry> Entries { get; set; } = new();
}
