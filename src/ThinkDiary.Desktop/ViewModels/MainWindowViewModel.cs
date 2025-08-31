using System.Threading.Tasks;
using ThinkDiary.Desktop.Services;

namespace ThinkDiary.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IConfigurationService _configurationService;
        private string _userName = "Loading...";

        public MainWindowViewModel()
        {
            _configurationService = new ConfigurationService();
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
    }
}