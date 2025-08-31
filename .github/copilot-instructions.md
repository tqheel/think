# ThinkDiary AI Coding Assistant Instructions

## Project Overview
ThinkDiary is an AI-enhanced personal diary application built with **Avalonia UI** (cross-platform), **Entity Framework Core**, and **Microsoft Semantic Kernel**. The project follows a **layered architecture** evolving from desktop to full-stack web application.

## Architecture & Key Patterns

### 🏗️ Solution Structure (Layered Architecture)
- **ThinkDiary.Core** - Domain models, interfaces, DTOs (no dependencies)
- **ThinkDiary.Data** - EF Core data layer, repositories, DbContext
- **ThinkDiary.AI** - Semantic Kernel integration, AI plugins/prompts
- **ThinkDiary.Desktop** - Avalonia UI (MVVM pattern)
- **ThinkDiary.API** - ASP.NET Core Web API (future)
- **ThinkDiary.Tests** - xUnit testing with FluentAssertions

### 📁 Critical File Patterns
- **Configuration**: `/src/config.json` (shared app settings, relative path resolution)
- **AI Prompts**: `/src/ThinkDiary.AI/Prompts/{FeatureName}/` (config.json + skprompt.txt)
- **Domain Models**: Rich entities with AI-generated metadata (Summary, Themes, SentimentScore)
- **Services**: Interface-first design, async patterns throughout

## Technology Specifics

### 🎯 Framework Versions
- **.NET 9.0** (all projects) - Use latest language features
- **Avalonia UI 11.3.4** - Cross-platform desktop (NOT WPF)
- **Entity Framework Core** - SQLite for local storage
- **Microsoft Semantic Kernel** - AI orchestration with local LLM support

### 🔧 Development Workflow Commands
```bash
# Build entire solution
dotnet build

# Run desktop app (from src/ThinkDiary.Desktop)
dotnet run

# Add EF migration (from src/ThinkDiary.Data)
dotnet ef migrations add MigrationName --startup-project ../ThinkDiary.Desktop

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
```

## Project-Specific Conventions

### 🎨 MVVM Pattern (Avalonia-specific)
- **ViewModels**: Inherit from `ViewModelBase`, use `SetProperty()` for binding
- **Services**: Interface-first, dependency injection ready
- **Configuration**: Use `IConfigurationService` for settings access
- **Data Binding**: Avalonia uses `x:DataType` for compiled bindings

### 🤖 AI Integration Architecture
- **Plugin Structure**: Each AI feature gets its own plugin class
- **Prompt Organization**: Structured as `/Prompts/{FeatureName}/config.json` + `skprompt.txt`
- **Local LLM**: Default endpoint `http://localhost:1234/v1` (LM Studio)
- **AI Metadata**: Domain models include optional AI-generated properties

### 📊 Domain Model Patterns
```csharp
// Rich domain entities with AI metadata
public class DiaryEntry {
    // Core properties
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Content { get; set; } = string.Empty; // Markdown content
    
    // AI-generated metadata (nullable)
    public string? Summary { get; set; }
    public List<string>? Themes { get; set; }
    public double? SentimentScore { get; set; }
}
```

### 🗄️ Data Access Patterns
- **Repository Pattern**: Generic `IRepository<T>` with entity-specific implementations
- **Unit of Work**: Transaction management across repositories
- **EF Configurations**: Separate configuration classes for complex entities
- **Migrations**: Named descriptively, startup project is ThinkDiary.Desktop

## Testing Strategy

### 🧪 Testing Structure
- **Unit Tests**: `/UnitTests/Models/`, `/UnitTests/Services/`
- **Test Framework**: xUnit + FluentAssertions for readable assertions
- **Coverage Target**: ≥90% for core models and services
- **CI Pipeline**: Multi-platform builds (Ubuntu, Windows, macOS)

### 📝 Test Patterns
```csharp
// Use FluentAssertions for readable tests
entry.Id.Should().NotBeEmpty();
entry.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
entry.Tags.Should().BeEmpty();
```

## Common Development Tasks

### 🆕 Adding New Features
1. Define interfaces in `ThinkDiary.Core/Interfaces/`
2. Implement in appropriate layer (Data/AI/Desktop)
3. Add unit tests with ≥90% coverage
4. Update CI pipeline if needed

### 🔌 AI Plugin Development
1. Create plugin class in `ThinkDiary.AI/Plugins/`
2. Add prompts in `ThinkDiary.AI/Prompts/{PluginName}/`
3. Register plugin in `SemanticKernelService`
4. Add interface contract in `ThinkDiary.Core`

### 🖥️ UI Development (Avalonia)
- Use **compiled bindings** with `x:DataType`
- Implement **MVVM** pattern consistently
- Follow **Material Design** principles
- Support **cross-platform** considerations

## Build & Deployment

### 📦 Package Management
- **Centralized**: Add packages at solution level when possible
- **Project References**: Core → Data → Desktop (no circular dependencies)
- **AI Packages**: Semantic Kernel, OpenAI connectors in ThinkDiary.AI only

### 🚀 CI/CD Pipeline
- **GitHub Actions**: `.github/workflows/ci.yml`
- **Multi-platform**: Ubuntu, Windows, macOS builds
- **Quality Gates**: Format checking, static analysis, coverage reporting
- **Triggers**: Push/PR to main/develop branches

Remember: This is a **learning project** focused on modern .NET development patterns, AI integration, and cross-platform desktop applications. Prioritize clean architecture, comprehensive testing, and maintainable code patterns.
