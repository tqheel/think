using Google.Cloud.Firestore;
using ThinkDiary.Core.Models;

namespace ThinkDiary.Data;

public class FirestoreService
{
    private readonly FirestoreDb _db;
    private const string DiaryEntriesCollection = "diaryEntries";
    private const string TagsCollection = "tags";

    public FirestoreService(FirestoreDb firestoreDb)
    {
        _db = firestoreDb;
    }

    #region DiaryEntry Operations

    public async Task<DiaryEntry> CreateEntryAsync(DiaryEntry entry)
    {
        entry.CreatedAt = DateTime.UtcNow;
        entry.UpdatedAt = DateTime.UtcNow;
        
        var docRef = _db.Collection(DiaryEntriesCollection).Document(entry.Id.ToString());
        await docRef.SetAsync(entry);
        
        return entry;
    }

    public async Task<DiaryEntry?> GetEntryAsync(Guid id)
    {
        var docRef = _db.Collection(DiaryEntriesCollection).Document(id.ToString());
        var snapshot = await docRef.GetSnapshotAsync();
        
        if (!snapshot.Exists)
            return null;

        var entry = snapshot.ConvertTo<DiaryEntry>();
        
        // Load associated tags
        if (entry.TagIds.Any())
        {
            entry.Tags = await GetTagsByIdsAsync(entry.TagIds);
        }
        
        return entry;
    }

    public async Task<IEnumerable<DiaryEntry>> GetEntriesAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        Query query = _db.Collection(DiaryEntriesCollection);
        
        if (startDate.HasValue)
        {
            query = query.WhereGreaterThanOrEqualTo("CreatedAt", startDate.Value);
        }
        
        if (endDate.HasValue)
        {
            query = query.WhereLessThanOrEqualTo("CreatedAt", endDate.Value);
        }
        
        query = query.OrderByDescending("CreatedAt");
        
        var snapshot = await query.GetSnapshotAsync();
        var entries = snapshot.Documents.Select(doc => doc.ConvertTo<DiaryEntry>()).ToList();
        
        // Load tags for all entries
        var allTagIds = entries.SelectMany(e => e.TagIds).Distinct().ToList();
        if (allTagIds.Any())
        {
            var tags = await GetTagsByIdsAsync(allTagIds);
            var tagDict = tags.ToDictionary(t => t.Id);
            
            foreach (var entry in entries)
            {
                entry.Tags = entry.TagIds
                    .Where(tagDict.ContainsKey)
                    .Select(id => tagDict[id])
                    .ToList();
            }
        }
        
        return entries;
    }

    public async Task<DiaryEntry> UpdateEntryAsync(DiaryEntry entry)
    {
        entry.UpdatedAt = DateTime.UtcNow;
        
        var docRef = _db.Collection(DiaryEntriesCollection).Document(entry.Id.ToString());
        await docRef.SetAsync(entry, SetOptions.MergeAll);
        
        return entry;
    }

    public async Task DeleteEntryAsync(Guid id)
    {
        var docRef = _db.Collection(DiaryEntriesCollection).Document(id.ToString());
        await docRef.DeleteAsync();
    }

    public async Task<IEnumerable<DiaryEntry>> SearchEntriesAsync(string query)
    {
        // Note: Firestore doesn't support full-text search directly
        // This is a basic implementation that searches in title and content
        // For production, consider using Firestore search extensions or external search service
        
        var allEntries = await GetEntriesAsync();
        
        return allEntries.Where(e => 
            e.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            e.Content.Contains(query, StringComparison.OrdinalIgnoreCase)) ?? Enumerable.Empty<DiaryEntry>();
    }

    #endregion

    #region Tag Operations

    public async Task<Tag> CreateTagAsync(Tag tag)
    {
        if (string.IsNullOrEmpty(tag.Id))
        {
            tag.Id = Guid.NewGuid().ToString();
        }
        
        var docRef = _db.Collection(TagsCollection).Document(tag.Id);
        await docRef.SetAsync(tag);
        
        return tag;
    }

    public async Task<Tag?> GetTagAsync(string id)
    {
        var docRef = _db.Collection(TagsCollection).Document(id);
        var snapshot = await docRef.GetSnapshotAsync();
        
        if (!snapshot.Exists)
            return null;

        return snapshot.ConvertTo<Tag>();
    }

    public async Task<IEnumerable<Tag>> GetAllTagsAsync()
    {
        var snapshot = await _db.Collection(TagsCollection).GetSnapshotAsync();
        return snapshot.Documents.Select(doc => doc.ConvertTo<Tag>());
    }

    public async Task<List<Tag>> GetTagsByIdsAsync(IEnumerable<string> tagIds)
    {
        var tags = new List<Tag>();
        
        foreach (var tagId in tagIds)
        {
            var tag = await GetTagAsync(tagId);
            if (tag != null)
            {
                tags.Add(tag);
            }
        }
        
        return tags;
    }

    public async Task<Tag?> GetTagByNameAsync(string name)
    {
        var query = _db.Collection(TagsCollection).WhereEqualTo("Name", name);
        var snapshot = await query.GetSnapshotAsync();
        
        return snapshot.Documents.FirstOrDefault()?.ConvertTo<Tag>();
    }

    public async Task<Tag> UpdateTagAsync(Tag tag)
    {
        var docRef = _db.Collection(TagsCollection).Document(tag.Id);
        await docRef.SetAsync(tag, SetOptions.MergeAll);
        
        return tag;
    }

    public async Task DeleteTagAsync(string id)
    {
        // First, remove the tag from all diary entries
        var entriesWithTag = await GetEntriesByTagIdAsync(id);
        foreach (var entry in entriesWithTag)
        {
            entry.TagIds.Remove(id);
            await UpdateEntryAsync(entry);
        }
        
        // Then delete the tag
        var docRef = _db.Collection(TagsCollection).Document(id);
        await docRef.DeleteAsync();
    }

    public async Task<IEnumerable<DiaryEntry>> GetEntriesByTagIdAsync(string tagId)
    {
        var query = _db.Collection(DiaryEntriesCollection).WhereArrayContains("TagIds", tagId);
        var snapshot = await query.GetSnapshotAsync();
        
        var entries = snapshot.Documents.Select(doc => doc.ConvertTo<DiaryEntry>()).ToList();
        
        // Load tags for all entries
        var allTagIds = entries.SelectMany(e => e.TagIds).Distinct().ToList();
        if (allTagIds.Any())
        {
            var tags = await GetTagsByIdsAsync(allTagIds);
            var tagDict = tags.ToDictionary(t => t.Id);
            
            foreach (var entry in entries)
            {
                entry.Tags = entry.TagIds
                    .Where(tagDict.ContainsKey)
                    .Select(id => tagDict[id])
                    .ToList();
            }
        }
        
        return entries;
    }

    #endregion
}