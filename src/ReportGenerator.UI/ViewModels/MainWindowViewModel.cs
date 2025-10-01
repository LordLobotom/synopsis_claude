using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ReportGenerator.Application.Services;
using ReportGenerator.Domain.Entities;
using Serilog;

namespace ReportGenerator.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ITemplateService _templateService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string _title = "Report Generator - Template Manager";

    [ObservableProperty]
    private ObservableCollection<ReportTemplate> _templates = new();

    [ObservableProperty]
    private ReportTemplate? _selectedTemplate;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    public ObservableCollection<PageOrientation> AvailableOrientations { get; } = new()
    {
        PageOrientation.Portrait,
        PageOrientation.Landscape
    };

    public ObservableCollection<PaperSize> AvailablePaperSizes { get; } = new()
    {
        PaperSize.A4,
        PaperSize.A5,
        PaperSize.Letter,
        PaperSize.Legal,
        PaperSize.Custom
    };

    public MainWindowViewModel(ITemplateService templateService, IServiceProvider serviceProvider)
    {
        _templateService = templateService;
        _serviceProvider = serviceProvider;
        LoadTemplatesAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private async Task LoadTemplatesAsync()
    {
        try
        {
            StatusMessage = "Loading templates...";
            var templates = await _templateService.GetAllTemplatesAsync();
            Templates = new ObservableCollection<ReportTemplate>(templates);
            StatusMessage = $"Loaded {Templates.Count} template(s)";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading templates: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task CreateNewTemplateAsync()
    {
        try
        {
            StatusMessage = "Creating new template...";
            var newTemplate = await _templateService.CreateTemplateAsync(
                "New Report",
                "A new report template",
                Environment.UserName);

            Templates.Add(newTemplate);
            SelectedTemplate = newTemplate;
            StatusMessage = "Template created successfully";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error creating template: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task DeleteTemplateAsync()
    {
        if (SelectedTemplate == null) return;

        try
        {
            StatusMessage = "Deleting template...";
            await _templateService.DeleteTemplateAsync(SelectedTemplate.Id);
            Templates.Remove(SelectedTemplate);
            SelectedTemplate = null;
            StatusMessage = "Template deleted successfully";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error deleting template: {ex.Message}";
        }
    }

    [RelayCommand]
    private void OpenDesigner()
    {
        Log.Information("OpenDesigner called");

        if (SelectedTemplate == null)
        {
            Log.Warning("OpenDesigner: SelectedTemplate is null");
            return;
        }

        try
        {
            Log.Information("OpenDesigner: Creating designer window for template {TemplateId} - {TemplateName}",
                SelectedTemplate.Id, SelectedTemplate.Name);

            Log.Debug("OpenDesigner: Creating ReportDesignerWindow instance");
            var designerWindow = new Views.ReportDesignerWindow();

            Log.Debug("OpenDesigner: Getting ReportDesignerViewModel from DI container");
            var designerViewModel = _serviceProvider.GetRequiredService<ReportDesignerViewModel>();

            Log.Debug("OpenDesigner: Setting DataContext");
            designerWindow.DataContext = designerViewModel;

            Log.Debug("OpenDesigner: Loading template async");
            _ = designerViewModel.LoadTemplateAsync(SelectedTemplate.Id);

            Log.Debug("OpenDesigner: Showing window");
            designerWindow.Show();

            StatusMessage = $"Opened designer for '{SelectedTemplate.Name}'";
            Log.Information("OpenDesigner: Successfully opened designer for '{TemplateName}'", SelectedTemplate.Name);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "OpenDesigner: Error opening designer for template {TemplateId}", SelectedTemplate?.Id);
            StatusMessage = $"Error opening designer: {ex.Message}";
        }
    }
}
