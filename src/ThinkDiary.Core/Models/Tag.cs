using Google.Cloud.Firestore;

namespace ThinkDiary.Core.Models;

[FirestoreData]
public class Tag
{
    [FirestoreProperty]
    public string Id { get; set; } = string.Empty; // Using string ID for Firestore compatibility
    [FirestoreProperty]
    public string Name { get; set; } = string.Empty;
    [FirestoreProperty]
    public string? Color { get; set; }

    // Navigation property - not stored in Firestore to avoid unbounded arrays
    public List<DiaryEntry> Entries { get; set; } = new();
}
