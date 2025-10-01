# Report Generator

A modern, cross-platform report generation application built with .NET 8 and Avalonia UI, following Clean Architecture principles.

## 🎯 Overview

This is a complete rewrite of the legacy SINEA Report Generator using modern technologies and architecture patterns. The application provides a visual report designer for creating professional reports with data binding, expressions, barcodes, and multiple export formats.

## ✨ Features (MVP)

- ✅ Visual template management interface
- ✅ Create, edit, and delete report templates
- ✅ SQLite database for template storage
- ✅ Clean Architecture with dependency injection
- ✅ MVVM pattern with data binding
- ✅ Expression engine with 68+ built-in functions
- ✅ Cross-platform support (Linux, Windows, macOS)

## 🏗️ Architecture

The application follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────┐
│   Presentation Layer (Avalonia UI)  │  ← User Interface
│           MVVM Pattern               │
├─────────────────────────────────────┤
│      Application Layer               │  ← Use Cases & Services
│      Business Logic                  │
├─────────────────────────────────────┤
│      Domain Layer                    │  ← Entities & Interfaces
│      Core Business Rules             │
├─────────────────────────────────────┤
│      Infrastructure Layer            │  ← Data Access & External
│      EF Core, Repositories           │
└─────────────────────────────────────┘
```

## 🚀 Quick Start

### Prerequisites

- .NET 8.0 SDK or later
- Any platform: Linux, Windows, or macOS

### Build & Run

```bash
# Clone the repository
cd /home/lord/source/synopsis_claude

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Run the application
dotnet run --project src/ReportGenerator.UI/ReportGenerator.UI.csproj
```

The application will create a `reports.db` SQLite database automatically on first run.

## 📦 Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 8.0 | Core framework |
| Avalonia UI | 11.3.6 | Cross-platform UI framework |
| Entity Framework Core | 9.0.9 | ORM for SQLite & SQL Server |
| CommunityToolkit.Mvvm | 8.2.1 | MVVM framework |
| NCalc | 1.3.8 | Expression evaluator |
| Dapper | 2.1.66 | High-performance queries |
| Serilog | 4.3.0 | Structured logging |
| xUnit | Latest | Unit testing |

## 📁 Project Structure

```
ReportGenerator/
├── src/
│   ├── ReportGenerator.Domain/          # Core domain entities & interfaces
│   ├── ReportGenerator.Application/      # Application services & use cases
│   ├── ReportGenerator.Infrastructure/   # Data access & external services
│   └── ReportGenerator.UI/               # Avalonia MVVM UI
├── tests/
│   └── ReportGenerator.Tests/            # Unit & integration tests
├── docs/                                 # Comprehensive documentation
│   ├── 01_System_Requirements_Specification.md
│   ├── 02_Technical_Architecture.md
│   ├── 03_Technology_Stack.md
│   ├── 04_Database_Schema_Design.md
│   ├── 05_API_Interface_Specifications.md
│   └── 06_Development_Roadmap.md
└── PROGRESS.md                           # Development progress tracker
```

## 📚 Documentation

Comprehensive documentation is available in the `/docs` directory:

1. **System Requirements** - Functional & non-functional requirements
2. **Technical Architecture** - Clean Architecture design & patterns
3. **Technology Stack** - Detailed technology choices & justifications
4. **Database Schema** - Complete database design & migrations
5. **API Specifications** - Interface contracts & DTOs
6. **Development Roadmap** - 24-month phased development plan

## 🎓 Key Design Decisions

### Why Avalonia UI?

Avalonia was chosen over WPF because:
- **Cross-platform**: Runs on Linux, Windows, and macOS
- **Modern**: Active development with regular updates
- **XAML-based**: Familiar for WPF/UWP developers
- **WSL-compatible**: Works in development environments without X server

### Why Clean Architecture?

Clean Architecture provides:
- **Testability**: Each layer can be tested independently
- **Maintainability**: Clear separation of concerns
- **Flexibility**: Easy to swap implementations
- **Dependency Inversion**: Core business logic has no external dependencies

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true

# Run specific test project
dotnet test tests/ReportGenerator.Tests/ReportGenerator.Tests.csproj
```

## 🔧 Development

### Adding a New Feature

1. Define domain entities in `ReportGenerator.Domain/Entities`
2. Add repository interface in `ReportGenerator.Domain/Interfaces`
3. Implement repository in `ReportGenerator.Infrastructure/Repositories`
4. Create application service in `ReportGenerator.Application/Services`
5. Build UI in `ReportGenerator.UI/Views` with corresponding ViewModel
6. Register dependencies in `App.axaml.cs`

### Database Migrations

```bash
# Add a new migration
dotnet ef migrations add MigrationName --project src/ReportGenerator.Infrastructure

# Update database
dotnet ef database update --project src/ReportGenerator.UI
```

## 📈 Roadmap

### Phase 1: MVP (Complete ✅)
- Template management
- Basic UI
- Database integration
- Core architecture

### Phase 2: Designer (Next)
- Visual report designer canvas
- Drag & drop elements
- Property editors
- Real-time preview

### Phase 3: Data & Expressions
- Data source configuration
- SQL query builder
- Expression editor with IntelliSense
- Data binding UI

### Phase 4: Export & Print
- Report rendering engine
- PNG/JPEG export
- Print preview
- Barcode/QR code generation

See `/docs/06_Development_Roadmap.md` for the complete 24-month plan.

## 🤝 Contributing

This project follows:
- **Clean Code** principles
- **SOLID** design principles
- **C# Coding Conventions** (Microsoft standards)
- **Git Flow** branching strategy

## 📄 License

[Specify your license here]

## 👥 Authors

- Initial design and implementation with AI assistance (Claude)
- Based on legacy SINEA Report Generator

## 🙏 Acknowledgments

- Original SINEA Report Generator team
- Avalonia UI community
- .NET community

---

**Status**: MVP Complete ✅
**Last Updated**: October 1, 2025
**Version**: 1.0.0-alpha
