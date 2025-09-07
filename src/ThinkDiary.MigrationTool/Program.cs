using ThinkDiary.Core.Models;
using ThinkDiary.Data;
using ThinkDiary.Data.Migration;

namespace ThinkDiary.MigrationTool;

/// <summary>
/// Simple console utility to demonstrate Firestore data operations and migration
/// </summary>
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("ThinkDiary Firestore Migration Tool");
        Console.WriteLine("===================================");

        try
        {
            // Create Firestore database instance
            Console.WriteLine("Connecting to Firestore...");
            var firestoreDb = await CreateFirestoreDatabase();
            
            var firestoreService = new FirestoreService(firestoreDb);
            var migrationService = new DataMigrationService(firestoreService);

            Console.WriteLine("Connection successful!");

            // Demonstrate creating sample data
            await CreateSampleData(firestoreService);

            // Validate the migration/setup
            Console.WriteLine("\nValidating Firestore setup...");
            var isValid = await migrationService.ValidateMigrationAsync();
            
            if (isValid)
            {
                Console.WriteLine("✓ Firestore setup validation passed!");
            }
            else
            {
                Console.WriteLine("✗ Firestore setup validation failed!");
            }

            // Display summary
            await DisplayDataSummary(firestoreService);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("\nTo use this tool, ensure:");
            Console.WriteLine("1. Firestore emulator is running (for testing): firebase emulators:start --only firestore");
            Console.WriteLine("2. Or configure valid Firestore credentials in config.json");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    private static async Task<Google.Cloud.Firestore.FirestoreDb> CreateFirestoreDatabase()
    {
        try
        {
            // Try to create from config first
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "src", "config.json");
            if (File.Exists(configPath))
            {
                return FirestoreFactory.CreateFirestoreDbFromConfig(configPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not load from config: {ex.Message}");
        }

        // Fall back to emulator for testing
        Console.WriteLine("Using Firestore emulator for testing...");
        Environment.SetEnvironmentVariable("FIRESTORE_EMULATOR_HOST", "localhost:8080");
        return Google.Cloud.Firestore.FirestoreDb.Create("thinkdiary-dev");
    }

    private static async Task CreateSampleData(FirestoreService firestoreService)
    {
        Console.WriteLine("\nCreating sample data...");

        // Create a sample tag
        var tag = new Tag
        {
            Name = "Sample Tag",
            Color = "#FF5722"
        };
        var createdTag = await firestoreService.CreateTagAsync(tag);
        Console.WriteLine($"Created tag: {createdTag.Name}");

        // Create a sample diary entry
        var entry = new DiaryEntry
        {
            Title = "Welcome to ThinkDiary with Firestore!",
            Content = "This is a sample diary entry created using the new Firestore data layer. The migration from Entity Framework Core to Firestore is now complete!",
            Mood = Mood.Happy,
            TagIds = new List<string> { createdTag.Id }
        };
        var createdEntry = await firestoreService.CreateEntryAsync(entry);
        Console.WriteLine($"Created entry: {createdEntry.Title}");
        Console.WriteLine($"Entry ID: {createdEntry.Id}");
        Console.WriteLine($"Word count: {createdEntry.WordCount}");
    }

    private static async Task DisplayDataSummary(FirestoreService firestoreService)
    {
        Console.WriteLine("\n=== Data Summary ===");
        
        var entries = await firestoreService.GetEntriesAsync();
        var tags = await firestoreService.GetAllTagsAsync();

        Console.WriteLine($"Total diary entries: {entries.Count()}");
        Console.WriteLine($"Total tags: {tags.Count()}");

        if (entries.Any())
        {
            Console.WriteLine("\nRecent entries:");
            foreach (var entry in entries.Take(3))
            {
                Console.WriteLine($"- {entry.Title} (Created: {entry.CreatedAt:yyyy-MM-dd HH:mm})");
            }
        }

        if (tags.Any())
        {
            Console.WriteLine("\nAvailable tags:");
            foreach (var tag in tags.Take(5))
            {
                Console.WriteLine($"- {tag.Name} ({tag.Color ?? "No color"})");
            }
        }
    }
}
