using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ThinkDiary.Desktop.Services
{
    public interface IConfigurationService
    {
        Task<string> GetUserNameAsync();
        Task<T> GetSettingAsync<T>(string key);
    }

    public class ConfigurationService : IConfigurationService
    {
        private readonly string _configPath;
        private JsonDocument? _config;

        public ConfigurationService()
        {
            _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "config.json");
        }

        private async Task LoadConfigAsync()
        {
            if (_config == null && File.Exists(_configPath))
            {
                var json = await File.ReadAllTextAsync(_configPath);
                _config = JsonDocument.Parse(json);
            }
        }

        public async Task<string> GetUserNameAsync()
        {
            await LoadConfigAsync();
            if (_config?.RootElement.TryGetProperty("UserName", out var userNameProperty) == true)
            {
                var configUserName = userNameProperty.GetString();
                if (!string.IsNullOrWhiteSpace(configUserName))
                {
                    return configUserName;
                }
            }
            
            // Fall back to system username if not configured or empty
            return Environment.GetEnvironmentVariable("USER") ?? 
                   Environment.GetEnvironmentVariable("USERNAME") ?? 
                   "Guest";
        }

        public async Task<T> GetSettingAsync<T>(string key)
        {
            await LoadConfigAsync();
            if (_config?.RootElement.TryGetProperty(key, out var property) == true)
            {
                return JsonSerializer.Deserialize<T>(property.GetRawText());
            }
            return default(T);
        }
    }
}
