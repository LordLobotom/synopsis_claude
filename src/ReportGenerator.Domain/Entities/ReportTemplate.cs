namespace ReportGenerator.Domain.Entities;

/// <summary>
/// Represents a report template containing sections and elements
/// </summary>
public class ReportTemplate
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public int Version { get; set; }

    // Template properties
    public PageOrientation Orientation { get; set; } = PageOrientation.Portrait;
    public PaperSize PaperSize { get; set; } = PaperSize.A4;
    public Margin Margins { get; set; } = new();

    // Sections
    public List<ReportSection> Sections { get; set; } = new();

    // Data source
    public Guid? DataSourceId { get; set; }
    public DataSource? DataSource { get; set; }
}

public enum PageOrientation
{
    Portrait,
    Landscape
}

public enum PaperSize
{
    A4,
    A5,
    Letter,
    Legal,
    Custom
}

public class Margin
{
    public double Top { get; set; } = 25.4; // mm
    public double Bottom { get; set; } = 25.4;
    public double Left { get; set; } = 25.4;
    public double Right { get; set; } = 25.4;
}
