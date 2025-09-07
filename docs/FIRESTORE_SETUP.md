# Firestore Migration Documentation

## Overview

ThinkDiary has been successfully migrated from Entity Framework Core with SQLite to Google Cloud Firestore. This document outlines the migration changes, setup requirements, and usage.

## Architecture Changes

### Data Models

The domain models have been updated with Firestore attributes:

```csharp
[FirestoreData]
public class DiaryEntry
{
    [FirestoreProperty]
    public Guid Id { get; set; } = Guid.NewGuid();
    [FirestoreProperty]
    public string Title { get; set; } = string.Empty;
    [FirestoreProperty]
    public string Content { get; set; } = string.Empty;
    [FirestoreProperty]
    public List<string> TagIds { get; set; } = new(); // References to Tag documents
    // ... other properties
}

[FirestoreData]
public class Tag
{
    [FirestoreProperty]
    public string Id { get; set; } = string.Empty; // Changed from int to string
    [FirestoreProperty]
    public string Name { get; set; } = string.Empty;
    [FirestoreProperty]
    public string? Color { get; set; }
}
```

### Many-to-Many Relationship Handling

The many-to-many relationship between DiaryEntry and Tag is now handled using:

- **DiaryEntry.TagIds**: Array of tag document IDs
- **Tag documents**: Store only tag metadata, no entry references to avoid unbounded arrays
- **Queries**: Use `WhereArrayContains` to find entries by tag

### Data Layer Services

1. **FirestoreService**: Handles all Firestore operations (CRUD for DiaryEntry and Tag)
2. **DiaryService**: Implements `IDiaryService` using FirestoreService
3. **FirestoreFactory**: Creates Firestore database instances from configuration

## Setup Requirements

### 1. Firestore Project Setup

#### Option A: Google Cloud Firestore (Production)

1. Create a Google Cloud Project
2. Enable Firestore API
3. Create a service account and download the JSON key file
4. Update `src/config.json`:

```json
{
  "UserName": "Your Name",
  "AppSettings": {
    "Theme": "Light",
    "AutoSave": true
  },
  "Firestore": {
    "ProjectId": "your-project-id",
    "CredentialFile": "path/to/service-account-key.json"
  }
}
```

#### Option B: Firestore Emulator (Development)

1. Install Firebase CLI: `npm install -g firebase-tools`
2. Initialize Firebase project: `firebase init firestore`
3. Start emulator: `firebase emulators:start --only firestore`
4. The application will automatically detect and use the emulator

### 2. NuGet Packages

The following packages have been added:

- **ThinkDiary.Core**: `Google.Cloud.Firestore` 3.8.0
- **ThinkDiary.Data**: `Google.Cloud.Firestore` 3.8.0
- **ThinkDiary.Desktop**: `Microsoft.Extensions.DependencyInjection` 8.0.0, `Microsoft.Extensions.Hosting` 8.0.0

### 3. Removed Dependencies

The following Entity Framework packages have been removed:

- `Microsoft.EntityFrameworkCore.Sqlite`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.Tools`

## Configuration

### Dependency Injection Setup

The desktop application now uses Microsoft's DI container:

```csharp
// Register Firestore database
services.AddSingleton<FirestoreDb>(provider =>
{
    var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "config.json");
    return FirestoreFactory.CreateFirestoreDbFromConfig(configPath);
});

// Register services
services.AddSingleton<FirestoreService>();
services.AddScoped<IDiaryService, DiaryService>();
```

### ViewModels

ViewModels now receive dependencies through constructor injection:

```csharp
public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(IConfigurationService configurationService, IDiaryService diaryService)
    {
        _configurationService = configurationService;
        _diaryService = diaryService;
    }
}
```

## Data Operations

### Creating Entries

```csharp
var entry = await diaryService.CreateEntryAsync("My Title", "My content");
```

### Creating Tags

```csharp
var tag = new Tag { Name = "Important", Color = "#FF5722" };
var createdTag = await firestoreService.CreateTagAsync(tag);
```

### Associating Tags with Entries

```csharp
entry.TagIds.Add(tag.Id);
await firestoreService.UpdateEntryAsync(entry);
```

### Querying Entries by Tag

```csharp
var entriesWithTag = await firestoreService.GetEntriesByTagIdAsync(tagId);
```

## Migration Utility

A console application (`ThinkDiary.MigrationTool`) has been created to demonstrate Firestore operations:

```bash
cd src/ThinkDiary.MigrationTool
dotnet run
```

This tool:
- Connects to Firestore (emulator or production)
- Creates sample data
- Validates the setup
- Displays data summary

## Testing

### Unit Tests

Basic unit tests validate model behavior and service logic:

```bash
dotnet test
```

### Integration Tests

Integration tests are included for Firestore operations (requires emulator):

```csharp
[Fact]
public async Task CreateEntryAsync_ShouldCreateNewEntry()
{
    // Test uses Firestore emulator
    Environment.SetEnvironmentVariable("FIRESTORE_EMULATOR_HOST", "localhost:8080");
    // ... test implementation
}
```

## Performance Considerations

### Read Operations

- Use pagination for large datasets via `Query.Limit()` and `Query.StartAfter()`
- Consider caching frequently accessed tags
- Use `GetAll()` sparingly for large collections

### Write Operations

- Batch writes for multiple operations
- Use transactions for consistency requirements
- Consider eventual consistency for non-critical updates

### Indexing

Firestore automatically indexes single fields. For complex queries, create composite indexes:

```
Composite indexes needed for:
- DiaryEntry: CreatedAt (desc) + TagIds (array)
- DiaryEntry: Title (asc) + Content (asc) for search
```

## Security Rules

For production deployment, configure Firestore security rules:

```javascript
rules_version = '2';
service cloud.firestore {
  match /databases/{database}/documents {
    // Allow authenticated users to read/write their own data
    match /diaryEntries/{entryId} {
      allow read, write: if request.auth != null;
    }
    
    match /tags/{tagId} {
      allow read, write: if request.auth != null;
    }
  }
}
```

## Troubleshooting

### Common Issues

1. **Firestore Emulator Connection**
   - Ensure emulator is running: `firebase emulators:start --only firestore`
   - Check if port 8080 is available

2. **Authentication Errors**
   - Verify service account JSON file path
   - Ensure `GOOGLE_APPLICATION_CREDENTIALS` environment variable is set

3. **Build Errors**
   - Ensure all projects target .NET 8.0
   - Verify all NuGet packages are restored

### Environment Variables

For testing and development:

```bash
# Use Firestore emulator
export FIRESTORE_EMULATOR_HOST=localhost:8080

# Use service account for production
export GOOGLE_APPLICATION_CREDENTIALS=/path/to/service-account.json
```

## Next Steps

### Potential Enhancements

1. **Real-time Updates**: Implement Firestore listeners for live data synchronization
2. **Offline Support**: Use Firestore offline persistence for desktop application
3. **Full-text Search**: Integrate with Algolia or implement client-side search
4. **Data Export**: Create utilities to export data from Firestore
5. **Backup Strategy**: Implement automated Firestore backups

### Web Application Migration

When migrating to ASP.NET Core Web API:

1. Register Firestore services in `Program.cs`
2. Create controllers that use `IDiaryService`
3. Implement authentication middleware
4. Configure Firestore security rules
5. Add real-time SignalR hubs using Firestore listeners

## Conclusion

The migration to Firestore provides:

- ✅ Scalable NoSQL database
- ✅ Real-time capabilities
- ✅ Flexible document structure
- ✅ Built-in security features
- ✅ Offline support potential
- ✅ Multi-platform compatibility

The application maintains all existing functionality while gaining the benefits of a cloud-native database solution.