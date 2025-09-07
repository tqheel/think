using System.Threading.Tasks;
using ThinkDiary.Core.Interfaces;
using ThinkDiary.Desktop.Services;

namespace ThinkDiary.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IDiaryService _diaryService;
        private string _userName = "Loading...";

        public MainWindowViewModel(IConfigurationService configurationService, IDiaryService diaryService)
        {
            _configurationService = configurationService;
            _diaryService = diaryService;
            _ = LoadUserNameAsync();
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private async Task LoadUserNameAsync()
        {
            UserName = await _configurationService.GetUserNameAsync();
        }

        // Example method using the diary service
        public async Task CreateTestEntryAsync()
        {
            var entry = await _diaryService.CreateEntryAsync("Test Entry", "This is a test entry created through Firestore!");
            // Handle the created entry
        }
    }
}