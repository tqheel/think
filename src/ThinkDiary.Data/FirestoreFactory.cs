using Google.Cloud.Firestore;
using System.Text.Json;

namespace ThinkDiary.Data;

public static class FirestoreFactory
{
    public static FirestoreDb CreateFirestoreDb(string projectId, string? credentialFile = null)
    {
        if (!string.IsNullOrEmpty(credentialFile) && File.Exists(credentialFile))
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialFile);
        }

        return FirestoreDb.Create(projectId);
    }

    public static FirestoreDb CreateFirestoreDbFromConfig(string configPath)
    {
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException($"Configuration file not found: {configPath}");
        }

        var configJson = File.ReadAllText(configPath);
        var config = JsonDocument.Parse(configJson);

        if (!config.RootElement.TryGetProperty("Firestore", out var firestoreConfig))
        {
            throw new InvalidOperationException("Firestore configuration not found in config file");
        }

        var projectId = firestoreConfig.GetProperty("ProjectId").GetString();
        if (string.IsNullOrEmpty(projectId))
        {
            throw new InvalidOperationException("Firestore ProjectId is required");
        }

        string? credentialFile = null;
        if (firestoreConfig.TryGetProperty("CredentialFile", out var credentialProperty))
        {
            credentialFile = credentialProperty.GetString();
        }

        return CreateFirestoreDb(projectId, credentialFile);
    }
}