using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReportGenerator.Application.Services;
using ReportGenerator.Domain.Entities;
using Serilog;

namespace ReportGenerator.UI.ViewModels;

public partial class ReportDesignerViewModel : ViewModelBase
{
    private readonly ITemplateService _templateService;

    [ObservableProperty]
    private ReportTemplate? _template;

    [ObservableProperty]
    private ReportSection? _selectedSection;

    [ObservableProperty]
    private ReportElement? _selectedElement;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    [ObservableProperty]
    private double _zoomLevel = 1.0;

    [ObservableProperty]
    private bool _showGrid = true;

    [ObservableProperty]
    private bool _snapToGrid = true;

    [ObservableProperty]
    private double _gridSize = 5.0; // mm

    public ObservableCollection<ElementType> AvailableElementTypes { get; } = new()
    {
        ElementType.Label,
        ElementType.TextField,
        ElementType.CalculatedField,
        ElementType.Line,
        ElementType.Rectangle,
        ElementType.RoundedRectangle,
        ElementType.Ellipse,
        ElementType.Image,
        ElementType.Barcode,
        ElementType.QRCode
    };

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

    public ObservableCollection<TextAlignment> AvailableTextAlignments { get; } = new()
    {
        TextAlignment.Left,
        TextAlignment.Center,
        TextAlignment.Right,
        TextAlignment.Justify
    };

    public ObservableCollection<VerticalAlignment> AvailableVerticalAlignments { get; } = new()
    {
        VerticalAlignment.Top,
        VerticalAlignment.Middle,
        VerticalAlignment.Bottom
    };

    public ObservableCollection<string> AvailableFontFamilies { get; } = new()
    {
        "Arial",
        "Calibri",
        "Courier New",
        "Georgia",
        "Tahoma",
        "Times New Roman",
        "Trebuchet MS",
        "Verdana"
    };

    public ReportDesignerViewModel(ITemplateService templateService)
    {
        Log.Information("ReportDesignerViewModel constructor called");
        _templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
        Log.Information("ReportDesignerViewModel created successfully");
    }

    public async Task LoadTemplateAsync(Guid templateId)
    {
        try
        {
            Log.Information("LoadTemplateAsync called for template {TemplateId}", templateId);
            StatusMessage = "Loading template...";

            Log.Debug("LoadTemplateAsync: Fetching template from service");
            Template = await _templateService.GetTemplateAsync(templateId);

            if (Template?.Sections.Any() == true)
            {
                Log.Debug("LoadTemplateAsync: Template has {SectionCount} sections, selecting first", Template.Sections.Count);
                SelectedSection = Template.Sections.First();
            }
            else
            {
                Log.Warning("LoadTemplateAsync: Template has no sections");
            }

            StatusMessage = $"Template '{Template?.Name}' loaded";
            Log.Information("LoadTemplateAsync: Template '{TemplateName}' loaded successfully", Template?.Name);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "LoadTemplateAsync: Error loading template {TemplateId}", templateId);
            StatusMessage = $"Error loading template: {ex.Message}";
        }
    }

    [RelayCommand]
    private void AddSection(SectionType sectionType)
    {
        if (Template == null) return;

        var newSection = new ReportSection
        {
            Id = Guid.NewGuid(),
            Name = $"{sectionType} Section",
            Type = sectionType,
            Height = 50,
            OrderIndex = Template.Sections.Count,
            TemplateId = Template.Id
        };

        Template.Sections.Add(newSection);
        SelectedSection = newSection;
        StatusMessage = $"Added {sectionType} section";
    }

    [RelayCommand]
    private void RemoveSection()
    {
        if (Template == null || SelectedSection == null) return;

        Template.Sections.Remove(SelectedSection);
        SelectedSection = Template.Sections.FirstOrDefault();
        StatusMessage = "Section removed";
    }

    [RelayCommand]
    private void AddElement(ElementType elementType)
    {
        if (SelectedSection == null) return;

        var newElement = new ReportElement
        {
            Id = Guid.NewGuid(),
            Name = $"{elementType}_{SelectedSection.Elements.Count + 1}",
            Type = elementType,
            SectionId = SelectedSection.Id,
            X = 10,
            Y = 10,
            Width = GetDefaultWidth(elementType),
            Height = GetDefaultHeight(elementType),
            FontFamily = "Arial",
            FontSize = 10,
            ForeColor = "#000000",
            BackColor = "#FFFFFF",
            Visible = true,
            ZIndex = SelectedSection.Elements.Count
        };

        // Set default content based on element type
        switch (elementType)
        {
            case ElementType.Label:
                newElement.StaticText = "Label";
                break;
            case ElementType.TextField:
                newElement.DataField = "[FieldName]";
                break;
            case ElementType.CalculatedField:
                newElement.Expression = "=SUM([Field])";
                break;
        }

        SelectedSection.Elements.Add(newElement);
        SelectedElement = newElement;
        StatusMessage = $"Added {elementType} element";
    }

    [RelayCommand]
    private void RemoveElement()
    {
        if (SelectedSection == null || SelectedElement == null) return;

        SelectedSection.Elements.Remove(SelectedElement);
        SelectedElement = SelectedSection.Elements.FirstOrDefault();
        StatusMessage = "Element removed";
    }

    [RelayCommand]
    private void DuplicateElement()
    {
        if (SelectedSection == null || SelectedElement == null) return;

        var duplicate = new ReportElement
        {
            Id = Guid.NewGuid(),
            Name = $"{SelectedElement.Name}_Copy",
            Type = SelectedElement.Type,
            SectionId = SelectedSection.Id,
            X = SelectedElement.X + 5,
            Y = SelectedElement.Y + 5,
            Width = SelectedElement.Width,
            Height = SelectedElement.Height,
            FontFamily = SelectedElement.FontFamily,
            FontSize = SelectedElement.FontSize,
            FontBold = SelectedElement.FontBold,
            FontItalic = SelectedElement.FontItalic,
            ForeColor = SelectedElement.ForeColor,
            BackColor = SelectedElement.BackColor,
            TextAlign = SelectedElement.TextAlign,
            VerticalAlign = SelectedElement.VerticalAlign,
            HasBorder = SelectedElement.HasBorder,
            BorderColor = SelectedElement.BorderColor,
            BorderWidth = SelectedElement.BorderWidth,
            StaticText = SelectedElement.StaticText,
            DataField = SelectedElement.DataField,
            Expression = SelectedElement.Expression,
            FormatString = SelectedElement.FormatString,
            Visible = SelectedElement.Visible,
            ZIndex = SelectedSection.Elements.Count
        };

        SelectedSection.Elements.Add(duplicate);
        SelectedElement = duplicate;
        StatusMessage = "Element duplicated";
    }

    [RelayCommand]
    private async Task SaveTemplateAsync()
    {
        if (Template == null) return;

        try
        {
            StatusMessage = "Saving template...";
            await _templateService.UpdateTemplateAsync(Template);
            StatusMessage = "Template saved successfully";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving template: {ex.Message}";
        }
    }

    [RelayCommand]
    private void ZoomIn()
    {
        if (ZoomLevel < 4.0)
        {
            ZoomLevel += 0.1;
            StatusMessage = $"Zoom: {ZoomLevel:P0}";
        }
    }

    [RelayCommand]
    private void ZoomOut()
    {
        if (ZoomLevel > 0.25)
        {
            ZoomLevel -= 0.1;
            StatusMessage = $"Zoom: {ZoomLevel:P0}";
        }
    }

    [RelayCommand]
    private void ZoomReset()
    {
        ZoomLevel = 1.0;
        StatusMessage = "Zoom: 100%";
    }

    [RelayCommand]
    private void ToggleGrid()
    {
        ShowGrid = !ShowGrid;
        StatusMessage = ShowGrid ? "Grid shown" : "Grid hidden";
    }

    [RelayCommand]
    private void ToggleSnapToGrid()
    {
        SnapToGrid = !SnapToGrid;
        StatusMessage = SnapToGrid ? "Snap to grid enabled" : "Snap to grid disabled";
    }

    private double GetDefaultWidth(ElementType elementType)
    {
        return elementType switch
        {
            ElementType.Label => 80,
            ElementType.TextField => 100,
            ElementType.CalculatedField => 100,
            ElementType.Line => 100,
            ElementType.Rectangle => 80,
            ElementType.RoundedRectangle => 80,
            ElementType.Ellipse => 60,
            ElementType.Image => 80,
            ElementType.Barcode => 100,
            ElementType.QRCode => 50,
            _ => 80
        };
    }

    private double GetDefaultHeight(ElementType elementType)
    {
        return elementType switch
        {
            ElementType.Label => 20,
            ElementType.TextField => 20,
            ElementType.CalculatedField => 20,
            ElementType.Line => 1,
            ElementType.Rectangle => 40,
            ElementType.RoundedRectangle => 40,
            ElementType.Ellipse => 60,
            ElementType.Image => 60,
            ElementType.Barcode => 30,
            ElementType.QRCode => 50,
            _ => 20
        };
    }

    public void MoveElement(double deltaX, double deltaY)
    {
        if (SelectedElement == null) return;

        var newX = SelectedElement.X + deltaX;
        var newY = SelectedElement.Y + deltaY;

        if (SnapToGrid)
        {
            newX = Math.Round(newX / GridSize) * GridSize;
            newY = Math.Round(newY / GridSize) * GridSize;
        }

        SelectedElement.X = Math.Max(0, newX);
        SelectedElement.Y = Math.Max(0, newY);
    }

    public void ResizeElement(double deltaWidth, double deltaHeight)
    {
        if (SelectedElement == null) return;

        var newWidth = SelectedElement.Width + deltaWidth;
        var newHeight = SelectedElement.Height + deltaHeight;

        if (SnapToGrid)
        {
            newWidth = Math.Round(newWidth / GridSize) * GridSize;
            newHeight = Math.Round(newHeight / GridSize) * GridSize;
        }

        SelectedElement.Width = Math.Max(5, newWidth);
        SelectedElement.Height = Math.Max(5, newHeight);
    }
}
