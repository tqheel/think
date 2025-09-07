# 🧪 Testing Documentation

This document outlines the testing patterns, conventions, and best practices for the ThinkDiary project.

## Overview

The ThinkDiary project uses **xUnit** as the testing framework with **FluentAssertions** for readable assertions. The test suite includes comprehensive unit tests for all core models with ≥90% code coverage.

## Test Statistics

- **Total Tests**: 148
- **DiaryEntry Model**: 47 tests
- **Tag Model**: 24 tests  
- **Mood Enum**: 77 tests
- **Code Coverage**: 100% line coverage for core models

## Project Structure

```
src/ThinkDiary.Tests/
├── ThinkDiary.Tests.csproj           # Test project configuration
├── UnitTests/                        # Unit test classes
│   └── Models/
│       ├── DiaryEntryTests.cs        # DiaryEntry model tests
│       ├── TagTests.cs               # Tag model tests
│       └── MoodTests.cs              # Mood enum tests
└── TestData/                         # Test utilities
    └── ModelBuilders.cs              # Test data builders
```

## Testing Patterns

### 1. Test Data Builders

Use the Builder pattern for creating consistent, reusable test data:

```csharp
// Basic usage
var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();
var tag = ModelBuilders.TagBuilder.CreateDefault();

// Specialized builders
var entryWithMood = ModelBuilders.DiaryEntryBuilder.CreateWithMood(Mood.Happy);
var tagWithColor = ModelBuilders.TagBuilder.CreateWithColor("#FF0000");
var entryWithAI = ModelBuilders.DiaryEntryBuilder.CreateWithAIMetadata();

// Empty instances
var emptyEntry = ModelBuilders.DiaryEntryBuilder.CreateEmpty();
```

### 2. FluentAssertions Style

Write readable assertions using FluentAssertions:

```csharp
// Property validation
entry.Id.Should().NotBeEmpty("Id should be automatically generated");
entry.Title.Should().Be("Test Entry");

// Collection assertions
entry.Tags.Should().NotBeNull().And.BeEmpty("Tags should be initialized as empty list");
entry.Tags.Should().HaveCount(2).And.Contain(tag1).And.Contain(tag2);

// DateTime assertions
entry.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

// Null handling
entry.Mood.Should().BeNull("Mood should be nullable and default to null");
entry.Summary.Should().Be(summary);

// Enum assertions
mood.Should().Be(Mood.Happy);
Enum.IsDefined(typeof(Mood), mood).Should().BeTrue();
```

### 3. Test Method Naming

Follow descriptive naming patterns:

```csharp
[Fact]
public void Constructor_ShouldInitializeWithDefaultValues()

[Theory]
[InlineData("Sample Title")]
[InlineData("")]
public void Title_SetAndGet_ShouldWorkCorrectly(string title)

[Fact]
public void Tags_ShouldAllowAddingAndRemovingTags()
```

### 4. Test Organization

#### Arrange-Act-Assert Pattern
```csharp
[Fact]
public void Mood_ShouldAcceptNullValue()
{
    // Arrange
    var entry = ModelBuilders.DiaryEntryBuilder.CreateWithMood(Mood.Happy);

    // Act
    entry.Mood = null;

    // Assert
    entry.Mood.Should().BeNull();
}
```

#### Theory Tests for Multiple Inputs
```csharp
[Theory]
[InlineData(Mood.Happy)]
[InlineData(Mood.Sad)]
[InlineData(Mood.Neutral)]
public void Mood_ShouldAcceptAllValidMoodValues(Mood mood)
{
    // Arrange & Act & Assert
    var entry = ModelBuilders.DiaryEntryBuilder.CreateDefault();
    entry.Mood = mood;
    entry.Mood.Should().Be(mood);
}
```

## Test Coverage Areas

### DiaryEntry Model Tests
- ✅ Property initialization and default values
- ✅ GUID generation for Id property  
- ✅ DateTime properties (CreatedAt, UpdatedAt)
- ✅ Collections (Tags, Themes) initialization
- ✅ Nullable properties behavior
- ✅ Property getters and setters
- ✅ WordCount property validation
- ✅ All properties independence
- ✅ Edge cases and boundary conditions

### Tag Model Tests
- ✅ Property initialization
- ✅ Name property requirements
- ✅ Optional Color property behavior
- ✅ Entries collection initialization
- ✅ Collection operations (add/remove)
- ✅ Property independence
- ✅ Special characters and Unicode support

### Mood Enum Tests
- ✅ All enum values accessibility
- ✅ String representations
- ✅ Enum value conversions
- ✅ Parsing (successful and failed cases)
- ✅ TryParse behavior
- ✅ Underlying integer values
- ✅ JSON serialization/deserialization
- ✅ IsDefined validation
- ✅ Integration with DiaryEntry

## Running Tests

### Command Line Options

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage

# Verbose output
dotnet test --verbosity detailed

# Filter by test name
dotnet test --filter "FullyQualifiedName~DiaryEntryTests"

# Filter by category
dotnet test --filter "TestCategory=Unit"

# Run specific test method
dotnet test --filter "Mood_ShouldHaveExpectedValues"
```

### Visual Studio / VS Code

- Use Test Explorer to run individual tests
- Right-click on test methods to run/debug
- Use Live Unit Testing for real-time feedback

## Code Coverage

### Viewing Coverage

```bash
# Generate coverage report
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage

# Coverage files are in XML format (Cobertura)
ls coverage/*/coverage.cobertura.xml
```

### Coverage Targets

- **Minimum**: 90% line coverage for core models
- **Current**: 100% line coverage achieved
- **Goal**: Maintain high coverage as codebase grows

## Continuous Integration

### GitHub Actions Workflow

The CI pipeline automatically:

1. **Multi-Platform Testing**: Ubuntu, Windows, macOS
2. **Code Quality Checks**: Formatting verification
3. **Static Analysis**: Build with warnings as errors
4. **Coverage Reporting**: Upload to Codecov
5. **Automated Testing**: On every PR and push

### Local Pre-commit Checks

```bash
# Format code
dotnet format

# Build with warnings as errors
dotnet build --configuration Release /p:TreatWarningsAsErrors=true

# Run all tests
dotnet test
```

## Best Practices

### 1. Test Independence
- Each test should be completely independent
- No shared state between tests
- Use fresh instances for each test

### 2. Descriptive Test Names
- Test method names should describe what is being tested
- Include the expected behavior
- Use consistent naming patterns

### 3. Single Responsibility
- Each test should verify one specific behavior
- If testing multiple scenarios, use Theory tests
- Keep tests focused and simple

### 4. Test Data
- Use builders for consistent test data
- Avoid magic numbers/strings
- Make test data meaningful but simple

### 5. Assertions
- Use FluentAssertions for readability
- Include meaningful assertion messages
- Test both positive and negative cases

### 6. Coverage
- Aim for high coverage but focus on quality
- Test edge cases and error conditions
- Don't test framework code, focus on business logic

## Adding New Tests

### For New Models

1. Create test class in `UnitTests/Models/`
2. Add builders to `TestData/ModelBuilders.cs`
3. Follow existing naming patterns
4. Test all public properties and methods
5. Include edge cases and error scenarios

### For New Features

1. Write tests first (TDD approach)
2. Use existing patterns and builders
3. Ensure good coverage of new code
4. Update documentation if needed

## Troubleshooting

### Common Issues

1. **Tests failing after model changes**: Update test data builders
2. **Coverage not generating**: Ensure `coverlet.collector` package is installed
3. **FluentAssertions not found**: Add `using FluentAssertions;` statement
4. **Theory tests not running**: Check InlineData format matches method parameters

### Debugging Tests

- Use debugger breakpoints in test methods
- Add `Console.WriteLine()` for debugging output
- Use `Should().And` chaining for multiple assertions
- Check test output window for detailed failure information

## Future Enhancements

As the project grows, consider adding:

- **Integration Tests**: Database and API testing
- **Performance Tests**: Load and stress testing  
- **UI Tests**: Automated UI testing with Playwright
- **Mutation Testing**: Verify test quality
- **Property-Based Testing**: Generate test cases automatically

---

This testing foundation provides a solid base for maintaining code quality and preventing regressions as the ThinkDiary application evolves.