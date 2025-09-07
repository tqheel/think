using ThinkDiary.Core.Interfaces;
using ThinkDiary.Core.Models;

namespace ThinkDiary.Data;

public class DiaryService : IDiaryService
{
    private readonly FirestoreService _firestoreService;

    public DiaryService(FirestoreService firestoreService)
    {
        _firestoreService = firestoreService;
    }

    public async Task<DiaryEntry> CreateEntryAsync(string title, string content)
    {
        var entry = new DiaryEntry
        {
            Title = title,
            Content = content,
            WordCount = CountWords(content)
        };

        return await _firestoreService.CreateEntryAsync(entry);
    }

    public async Task<DiaryEntry?> GetEntryAsync(Guid id)
    {
        return await _firestoreService.GetEntryAsync(id);
    }

    public async Task<IEnumerable<DiaryEntry>> GetEntriesAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        return await _firestoreService.GetEntriesAsync(startDate, endDate);
    }

    public async Task<DiaryEntry> UpdateEntryAsync(DiaryEntry entry)
    {
        entry.WordCount = CountWords(entry.Content);
        return await _firestoreService.UpdateEntryAsync(entry);
    }

    public async Task DeleteEntryAsync(Guid id)
    {
        await _firestoreService.DeleteEntryAsync(id);
    }

    public async Task<IEnumerable<DiaryEntry>> SearchEntriesAsync(string query)
    {
        return await _firestoreService.SearchEntriesAsync(query);
    }

    private static int CountWords(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return 0;

        return content.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}