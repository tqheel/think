using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;

namespace ThinkDiary.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel = new ViewModels.MainWindowViewModel();

            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}