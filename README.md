# 📓 ThinkDiary - An AI-Enhanced Diary Application

A modern, intelligent diary application built with C#/.NET 8 and Microsoft Semantic Kernel that helps you capture, analyze, and reflect on your thoughts. This project starts as a desktop application and will evolve into a full-stack solution with Angular.

## 🎯 Project Goals

This project is designed as a comprehensive learning journey through:
- **C#/.NET 8** modern application development
- **Microsoft Semantic Kernel** for AI orchestration
- **Local LLM integration** for privacy-preserving AI features
- **Progressive architecture** - evolving from desktop to web
- Creating something genuinely useful while learning cutting-edge tech

## ✨ Features

### Core Features (MVP)
- **Simple Entry Creation**: Quick creation of diary entries with automatic date/time stamping
- **Markdown Editor**: Rich text editing with Markdown support and live preview
- **AI-Powered Insights**: Mood analysis, theme extraction, and reflection prompts
- **Semantic Search**: Find entries using natural language queries
- **Local Storage**: SQLite database with Entity Framework Core
- **Export Options**: Export entries as Markdown, PDF, or Word documents

### AI Features (Powered by Semantic Kernel)
- **Smart Summaries**: AI-generated daily/weekly/monthly summaries
- **Mood Tracking**: Automatic emotion detection and trends
- **Reflection Assistant**: Thoughtful questions based on your entries
- **Topic Clustering**: Discover recurring themes in your writing
- **Writing Suggestions**: Context-aware prompts and completions
- **Memory Chains**: Connect related thoughts across time

## 🛠️ Technology Stack

### Phase 1: Desktop Application
- **Framework**: .NET 8 (latest LTS)
- **UI**: WPF with Material Design or Avalonia UI (cross-platform)
- **AI Orchestration**: Microsoft Semantic Kernel
- **Database**: SQLite with Entity Framework Core 8
- **Local LLM**: Integration with LM Studio or Ollama

### Phase 2: Web Evolution
- **Backend**: ASP.NET Core 8 Web API
- **Frontend**: Angular 17+ with Material Design
- **Real-time**: SignalR for live updates
- **API**: RESTful with OpenAPI/Swagger
- **Authentication**: ASP.NET Core Identity

### Core NuGet Packages

```xml
<!-- .csproj package references -->
<PackageReference Include="Microsoft.SemanticKernel" Version="1.20.0" />
<PackageReference Include="Microsoft.SemanticKernel.Connectors.OpenAI" Version="1.20.0" />
<PackageReference Include="Microsoft.SemanticKernel.Plugins.Memory" Version="1.20.0-alpha" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="Markdig" Version="0.37.0" />
<PackageReference Include="MaterialDesignThemes" Version="5.0.0" />
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
```

## 🏗️ Architecture

### Solution Structure

```
ThinkDiary/
├── ThinkDiary.sln
├── src/
│   ├── ThinkDiary.Core/              # Domain models & interfaces
│   │   ├── ThinkDiary.Core.csproj
│   │   ├── Models/
│   │   │   ├── DiaryEntry.cs
│   │   │   ├── Mood.cs
│   │   │   └── Tag.cs
│   │   ├── Interfaces/
│   │   │   ├── IDiaryService.cs
│   │   │   └── IAIService.cs
│   │   └── DTOs/
│   │
│   ├── ThinkDiary.AI/                # Semantic Kernel integration
│   │   ├── ThinkDiary.AI.csproj
│   │   ├── KernelBuilder.cs
│   │   ├── Plugins/
│   │   │   ├── DiaryAnalyzerPlugin.cs
│   │   │   ├── MoodDetectorPlugin.cs
│   │   │   ├── ReflectionPlugin.cs
│   │   │   └── SummarizerPlugin.cs
│   │   ├── Prompts/
│   │   │   ├── AnalyzeEntry/
│   │   │   │   ├── config.json
│   │   │   │   └── skprompt.txt
│   │   │   └── GenerateQuestions/
│   │   └── Services/
│   │       └── SemanticKernelService.cs
│   │
│   ├── ThinkDiary.Data/              # Data access layer
│   │   ├── ThinkDiary.Data.csproj
│   │   ├── DiaryDbContext.cs
│   │   ├── Migrations/
│   │   ├── Repositories/
│   │   │   ├── DiaryEntryRepository.cs
│   │   │   └── TagRepository.cs
│   │   └── Configurations/
│   │       └── DiaryEntryConfiguration.cs
│   │
│   ├── ThinkDiary.Desktop/           # WPF Desktop App
│   │   ├── ThinkDiary.Desktop.csproj
│   │   ├── App.xaml
│   │   ├── App.xaml.cs
│   │   ├── MainWindow.xaml
│   │   ├── Views/
│   │   │   ├── EntryEditorView.xaml
│   │   │   ├── EntryListView.xaml
│   │   │   └── SettingsView.xaml
│   │   ├── ViewModels/
│   │   │   ├── MainViewModel.cs
│   │   │   ├── EntryViewModel.cs
│   │   │   └── SettingsViewModel.cs
│   │   └── Services/
│   │
│   ├── ThinkDiary.API/               # Web API (Phase 2)
│   │   ├── ThinkDiary.API.csproj
│   │   ├── Program.cs
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   └── Hubs/
│   │       └── DiaryHub.cs
│   │
│   └── ThinkDiary.Tests/
│       ├── ThinkDiary.Tests.csproj
│       ├── UnitTests/
│       └── IntegrationTests/
│
├── client/                           # Angular App (Phase 2)
│   ├── package.json
│   ├── angular.json
│   └── src/
│       ├── app/
│       └── environments/
│
├── docs/
├── scripts/
└── README.md
```

### Data Models

```csharp
// Core domain models
public class DiaryEntry
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }  // Markdown text
    public List<Tag> Tags { get; set; }
    public Mood? Mood { get; set; }
    public int WordCount { get; set; }
    
    // AI-generated metadata
    public string? Summary { get; set; }
    public List<string>? Themes { get; set; }
    public double? SentimentScore { get; set; }
}

public enum Mood
{
    Happy,
    Neutral,
    Sad,
    Excited,
    Anxious,
    Grateful,
    Reflective,
    Motivated
}

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Color { get; set; }
    public List<DiaryEntry> Entries { get; set; }
}
```

### Semantic Kernel Integration

```csharp
// Example of Semantic Kernel setup
public class SemanticKernelService
{
    private readonly IKernel _kernel;
    
    public SemanticKernelService(IConfiguration configuration)
    {
        _kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: "gpt-3.5-turbo",
                endpoint: new Uri("http://localhost:1234/v1"), // LM Studio
                apiKey: "not-needed-for-local")
            .AddPlugin<DiaryAnalyzerPlugin>()
            .AddPlugin<MoodDetectorPlugin>()
            .Build();
    }
    
    public async Task<DiaryAnalysis> AnalyzeEntryAsync(DiaryEntry entry)
    {
        var result = await _kernel.InvokeAsync(
            "DiaryAnalyzer", 
            "AnalyzeEntry",
            new() { ["input"] = entry.Content });
            
        return JsonSerializer.Deserialize<DiaryAnalysis>(result.ToString());
    }
}
```

## 🚀 Getting Started

### Prerequisites

- **.NET 8 SDK** (download from [dot.net](https://dotnet.microsoft.com/download/dotnet/8.0))
- **Visual Studio 2022** or **Visual Studio Code** with C# extension
- **LM Studio** or **Ollama** for local LLM (optional but recommended)
- **Node.js 18+** and Angular CLI (for Phase 2)

### Installation

```bash
# Clone the repository
git clone https://github.com/yourusername/ThinkDiary.git
cd ThinkDiary

# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build

# Run the desktop application
cd src/ThinkDiary.Desktop
dotnet run
```

### Setting up Local LLM (LM Studio)

1. Download [LM Studio](https://lmstudio.ai/)
2. Download a model (e.g., Llama 3, Mistral, or Phi-3)
3. Start the local server (usually on `http://localhost:1234`)
4. Update `appsettings.json` with your endpoint

### Development

```bash
# Run with hot reload
dotnet watch run

# Run tests
dotnet test

# Generate EF Core migration
cd src/ThinkDiary.Data
dotnet ef migrations add InitialCreate

# Apply database migrations
dotnet ef database update

# Format code
dotnet format
```

## 🧪 Testing

The project includes comprehensive unit tests for all core models with ≥90% code coverage.

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage

# Run tests with detailed output
dotnet test --verbosity detailed

# Run specific test class
dotnet test --filter "FullyQualifiedName~DiaryEntryTests"
```

### Test Structure

```
src/ThinkDiary.Tests/
├── UnitTests/
│   └── Models/
│       ├── DiaryEntryTests.cs    # 47 tests for DiaryEntry model
│       ├── TagTests.cs           # 24 tests for Tag model
│       └── MoodTests.cs          # 77 tests for Mood enum
└── TestData/
    └── ModelBuilders.cs          # Test data builders
```

### Testing Patterns

#### Test Data Builders
The project uses the Builder pattern for creating consistent test data:

```csharp
// Create default diary entry
var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();

// Create entry with specific mood
var happyEntry = ModelBuilders.DiaryEntryBuilder.CreateWithMood(Mood.Happy);

// Create tag with color
var tag = ModelBuilders.TagBuilder.CreateWithColor("#FF0000");
```

#### FluentAssertions
Tests use FluentAssertions for readable assertions:

```csharp
entry.Id.Should().NotBeEmpty("Id should be automatically generated");
entry.Tags.Should().NotBeNull().And.BeEmpty("Tags should be initialized as empty list");
entry.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
```

#### Test Coverage
- **DiaryEntry**: All properties, collections, nullable behavior, edge cases
- **Tag**: Property validation, collection operations, relationships
- **Mood**: Enum values, parsing, serialization, conversions

### Continuous Integration
GitHub Actions workflow automatically:
- Runs tests on Ubuntu, Windows, macOS
- Checks code formatting
- Generates coverage reports
- Uploads coverage to Codecov

## 🎨 UI Design Concepts

### Layout

```
┌─────────────────────────────────────────────────────┐
│  📓 ThinkDiary                              [−][□][×]│
├───────────────┬─────────────────────────────────────┤
│               │  Today, December 30, 2024            │
│  [+ New]      │ ┌─────────────────────────────────┐ │
│               │ │ Title: Morning Reflections      │ │
│  Entries:     │ └─────────────────────────────────┘ │
│               │                                      │
│  📅 Today (1) │ ┌─────────────┬───────────────────┐ │
│  📅 Yesterday │ │   Editor    │   Preview         │ │
│  📅 This Week │ │             │                   │ │
│               │ │ # Morning   │ Morning           │ │
│  [Search...]  │ │ Reflections │ Reflections       │ │
│               │ │             │                   │ │
│  Tags:        │ │ Today I     │ Today I learned...│ │
│  #personal    │ │ learned...  │                   │ │
│  #work        │ │             │                   │ │
│  #ideas       │ │             │                   │ │
│               │ └─────────────┴───────────────────┘ │
│               │                                      │
│               │ [💾 Save] [📤 Export] [🏷️ Tags]     │
└───────────────┴─────────────────────────────────────┘
```

### Key Interactions

1. **Quick Entry**: Ctrl/Cmd + N to create new entry
2. **Search**: Ctrl/Cmd + F to search all entries
3. **Save**: Auto-save every 30 seconds, manual save with Ctrl/Cmd + S
4. **Navigation**: Arrow keys or mouse to browse entries
5. **Markdown Shortcuts**: Toolbar or keyboard shortcuts for common formatting

## 🎓 Learning Resources

### Markdown Processing
- [Markdig Documentation](https://github.com/xoofx/markdig)
- [CommonMark Specification](https://commonmark.org/)

## 🤝 Contributing

This is primarily a learning project, but contributions are welcome! Feel free to:
- Report bugs
- Suggest features
- Submit pull requests
- Share your own diary app implementations

## 📝 License

MIT License - feel free to use this project as a starting point for your own learning journey!

## 🚧 Development Roadmap

### Phase 1: Basic Functionality (Week 1-2)
- [ ] Set up project structure
- [ ] Create basic WPF/Avalonia window
- [ ] Implement entry creation and viewing
- [ ] Add SQLite database integration
- [ ] Basic Markdown rendering

### Phase 2: Core Features (Week 3-4)
- [ ] Live Markdown preview
- [ ] Search functionality
- [ ] Entry list with date filtering
- [ ] Auto-save mechanism
- [ ] Basic export (Markdown files)

### Phase 3: Enhanced UX (Week 5-6)
- [ ] Tag system
- [ ] Keyboard shortcuts
- [ ] Settings/preferences
- [ ] Theme switching
- [ ] Better Markdown editor (syntax highlighting)

### Phase 4: Advanced Features (Week 7-8)
- [ ] Statistics dashboard
- [ ] Mood tracking
- [ ] PDF export
- [ ] Entry encryption
- [ ] Templates system

## 🚀 Future Feature Ideas

### AI-Powered Journaling Assistant (Local LLM Integration)

**Vision**: Integrate with a locally-running LLM (via LM Studio or similar) to provide intelligent, privacy-preserving journaling assistance.

#### Features:
- **Reflection Prompts**: AI generates thoughtful questions based on your recent entries
- **Mood Analysis**: Detect patterns in emotional states over time
- **Writing Suggestions**: Complete sentences, expand thoughts, improve clarity
- **Smart Summaries**: Generate weekly/monthly summaries of your entries
- **Topic Extraction**: Automatically identify and tag recurring themes
- **Private Chat**: Ask questions about your past entries ("What was I worried about last month?")

### Voice-to-Text Entry
- **Whisper Integration**: Use OpenAI's Whisper model locally for voice transcription
- **Real-time Transcription**: Speak your thoughts, see them appear as text
- **Voice Commands**: Navigate and control the app with voice
- **Audio Journaling**: Store audio clips alongside text entries

### Advanced Visualization & Analytics
- **Sentiment Timeline**: Visual representation of emotional journey
- **Word Clouds**: See most frequent topics/words over time
- **Connection Graph**: Visualize relationships between entries, tags, and themes
- **Writing Habits**: Track best times for journaling, streak analytics
- **Year in Review**: Auto-generated annual report with insights

### Smart Organization
- **Auto-Categorization**: ML-powered automatic tagging
- **Smart Search**: Semantic search that understands intent
- **Related Entries**: "You might also want to read..." suggestions
- **Memory Chains**: Link related thoughts across time

### Privacy & Security Enhancements
- **Local Encryption**: Client-side encryption with hardware key support
- **Plausible Deniability**: Hidden entries with separate password
- **Secure Sync**: P2P encrypted sync between your devices only
- **Biometric Lock**: TouchID/FaceID support on supported platforms

### Creative Tools
- **Prompt Library**: Curated writing prompts for different journaling styles
- **Drawing Canvas**: Sketch alongside your text entries
- **Photo Integration**: Attach and annotate images
- **Mind Mapping**: Visual brainstorming mode

### Integration Ecosystem
- **Git Backend**: Version control for entries
- **Obsidian Sync**: Export/import with Obsidian vault format
- **Calendar Integration**: See diary entries alongside calendar events
- **Health Data**: Correlate mood with sleep, exercise (via Apple Health, etc.)

### Companion Features
- **Web Clipper**: Save interesting articles/quotes to your diary
- **Email-to-Diary**: Send entries via email when away from computer
- **CLI Interface**: Quick entry creation from terminal

### Social & Sharing (Optional, Privacy-First)
- **Anonymous Sharing**: Share entries anonymously with community
- **Private Groups**: Shared journals with trusted friends/family
- **Export Stories**: Convert entry series into blog posts or books

## 💡 Design Philosophy

- **Privacy First**: Your thoughts stay on your machine
- **Simplicity**: Clean, distraction-free interface
- **Speed**: Instant startup and responsive interaction
- **Reliability**: Your entries are always safe and accessible
- **Learning Friendly**: Well-documented code for educational purposes

---

*"The life of every man is a diary in which he means to write one story, and writes another." - J.M. Barrie*

Happy journaling and happy coding! 📝