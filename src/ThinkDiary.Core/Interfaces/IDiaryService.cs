using ThinkDiary.Core.Models;

namespace ThinkDiary.Core.Interfaces;

public interface IDiaryService
{
    Task<DiaryEntry> CreateEntryAsync(string title, string content);
    Task<DiaryEntry?> GetEntryAsync(Guid id);
    Task<IEnumerable<DiaryEntry>> GetEntriesAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<DiaryEntry> UpdateEntryAsync(DiaryEntry entry);
    Task DeleteEntryAsync(Guid id);
    Task<IEnumerable<DiaryEntry>> SearchEntriesAsync(string query);
}
