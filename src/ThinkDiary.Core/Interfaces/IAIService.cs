using ThinkDiary.Core.Models;

namespace ThinkDiary.Core.Interfaces;

public interface IAIService
{
    Task<Mood?> DetectMoodAsync(string content);
    Task<string> GenerateSummaryAsync(string content);
    Task<List<string>> ExtractThemesAsync(string content);
    Task<double> AnalyzeSentimentAsync(string content);
    Task<List<string>> GenerateReflectionQuestionsAsync(DiaryEntry entry);
}
