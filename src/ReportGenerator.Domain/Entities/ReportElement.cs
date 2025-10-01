namespace ReportGenerator.Domain.Entities;

/// <summary>
/// Base class for all report elements (labels, fields, shapes, etc.)
/// </summary>
public class ReportElement
{
    public Guid Id { get; set; }
    public Guid SectionId { get; set; }
    public ReportSection? Section { get; set; }

    public string Name { get; set; } = string.Empty;
    public ElementType Type { get; set; }

    // Position and size (in mm)
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }

    // Appearance
    public string FontFamily { get; set; } = "Arial";
    public double FontSize { get; set; } = 10;
    public bool FontBold { get; set; }
    public bool FontItalic { get; set; }
    public string ForeColor { get; set; } = "#000000";
    public string BackColor { get; set; } = "#FFFFFF";
    public TextAlignment TextAlign { get; set; } = TextAlignment.Left;
    public VerticalAlignment VerticalAlign { get; set; } = VerticalAlignment.Top;

    // Border
    public bool HasBorder { get; set; }
    public string BorderColor { get; set; } = "#000000";
    public double BorderWidth { get; set; } = 1;

    // Content
    public string? StaticText { get; set; }
    public string? DataField { get; set; }
    public string? Expression { get; set; }
    public string? FormatString { get; set; }

    // Visibility
    public bool Visible { get; set; } = true;
    public string? VisibilityExpression { get; set; }

    // Specific properties stored as JSON
    public string? Properties { get; set; }

    public int ZIndex { get; set; }
}

public enum ElementType
{
    Label,
    TextField,
    CalculatedField,
    Line,
    Rectangle,
    RoundedRectangle,
    Ellipse,
    Image,
    Barcode,
    QRCode,
    SubReport
}

public enum TextAlignment
{
    Left,
    Center,
    Right,
    Justify
}

public enum VerticalAlignment
{
    Top,
    Middle,
    Bottom
}
