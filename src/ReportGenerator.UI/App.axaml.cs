using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReportGenerator.Application.Services;
using ReportGenerator.Domain.Interfaces;
using ReportGenerator.Infrastructure.Data;
using ReportGenerator.Infrastructure.Repositories;
using ReportGenerator.Infrastructure.Services;
using ReportGenerator.UI.ViewModels;
using ReportGenerator.UI.Views;
using Serilog;

namespace ReportGenerator.UI;

public partial class App : Avalonia.Application
{
    public IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/reportgenerator-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("=== Report Generator Starting ===");
        Log.Information("Initializing application");

        AvaloniaXamlLoader.Load(this);
        ConfigureServices();

        Log.Information("Application initialized successfully");
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Log.Information("Framework initialization starting");

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Log.Debug("Desktop lifetime detected");

            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            Log.Debug("Disabling Avalonia data annotation validation");
            DisableAvaloniaDataAnnotationValidation();

            // Initialize database
            Log.Information("Initializing database");
            InitializeDatabase();
            Log.Information("Database initialized");

            Log.Debug("Creating MainWindow");
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services!.GetRequiredService<MainWindowViewModel>(),
            };
            Log.Information("MainWindow created successfully");
        }

        base.OnFrameworkInitializationCompleted();
        Log.Information("Framework initialization completed");
    }

    private void ConfigureServices()
    {
        Log.Information("Configuring services");
        var services = new ServiceCollection();

        // Database
        Log.Debug("Registering DbContext");
        services.AddDbContext<ReportDbContext>(options =>
            options.UseSqlite("Data Source=reports.db"));

        // Repositories
        Log.Debug("Registering repositories");
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddScoped<IDataSourceRepository, DataSourceRepository>();
        services.AddScoped<IConnectionStringRepository, ConnectionStringRepository>();

        // Domain Services
        Log.Debug("Registering domain services");
        services.AddScoped<IExpressionEvaluator, ExpressionEvaluatorService>();

        // Application Services
        Log.Debug("Registering application services");
        services.AddScoped<ITemplateService, TemplateService>();

        // ViewModels
        Log.Debug("Registering ViewModels");
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ReportDesignerViewModel>();

        Log.Debug("Building service provider");
        Services = services.BuildServiceProvider();
        Log.Information("Services configured successfully");
    }

    private void InitializeDatabase()
    {
        using var scope = Services!.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
        dbContext.Database.EnsureCreated();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}