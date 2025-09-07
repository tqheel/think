using ThinkDiary.Core.Models;

namespace ThinkDiary.Data.Migration;

public class DataMigrationService
{
    private readonly FirestoreService _firestoreService;

    public DataMigrationService(FirestoreService firestoreService)
    {
        _firestoreService = firestoreService;
    }

    public async Task<MigrationResult> MigrateAllDataAsync()
    {
        var result = new MigrationResult();

        try
        {
            // Note: EF Core migration is now disabled since we removed Entity Framework dependencies
            // This method is kept for future use when actual data migration is needed
            
            result.IsSuccess = true;
            result.Message = "Migration completed. No EF Core data to migrate - using Firestore directly.";
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = $"Migration failed: {ex.Message}";
        }

        return result;
    }

    public async Task<bool> ValidateMigrationAsync()
    {
        try
        {
            // Validate that Firestore service is working
            var firestoreEntries = await _firestoreService.GetEntriesAsync();
            var firestoreTags = await _firestoreService.GetAllTagsAsync();

            var firestoreEntriesCount = firestoreEntries.Count();
            var firestoreTagsCount = firestoreTags.Count();

            Console.WriteLine($"Firestore: {firestoreEntriesCount} entries, {firestoreTagsCount} tags");

            return true; // Validation passes if we can read from Firestore
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Validation failed: {ex.Message}");
            return false;
        }
    }
}

public class MigrationResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int EntriesMigrated { get; set; }
    public int TagsMigrated { get; set; }
    public Dictionary<string, string> TagMappings { get; set; } = new();
}

public class TagMigrationResult
{
    public int TagsMigrated { get; set; }
    public Dictionary<string, string> TagMappings { get; set; } = new();
}

public class EntryMigrationResult
{
    public int EntriesMigrated { get; set; }
}