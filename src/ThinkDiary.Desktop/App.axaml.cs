using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Google.Cloud.Firestore;
using ThinkDiary.Core.Interfaces;
using ThinkDiary.Data;
using ThinkDiary.Desktop.Services;

namespace ThinkDiary.Desktop;

public partial class App : Application
{
    private IHost? _host;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Set up dependency injection
        _host = CreateHost();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel = _host.Services.GetRequiredService<ViewModels.MainWindowViewModel>();

            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static IHost CreateHost()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Register configuration service
                services.AddSingleton<IConfigurationService, ConfigurationService>();

                // Register Firestore database
                services.AddSingleton<FirestoreDb>(provider =>
                {
                    try
                    {
                        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "config.json");
                        return FirestoreFactory.CreateFirestoreDbFromConfig(configPath);
                    }
                    catch (Exception ex)
                    {
                        // For development, create a test instance
                        Console.WriteLine($"Failed to create Firestore from config: {ex.Message}");
                        Console.WriteLine("Using test Firestore instance");
                        Environment.SetEnvironmentVariable("FIRESTORE_EMULATOR_HOST", "localhost:8080");
                        return FirestoreDb.Create("thinkdiary-dev");
                    }
                });

                // Register Firestore service
                services.AddSingleton<FirestoreService>();

                // Register business services
                services.AddScoped<IDiaryService, DiaryService>();

                // Register ViewModels
                services.AddTransient<ViewModels.MainWindowViewModel>();
            })
            .Build();
    }
}