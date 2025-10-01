namespace ReportGenerator.Domain.Entities;

/// <summary>
/// Represents a section in a report (Header, Detail, Footer, etc.)
/// </summary>
public class ReportSection
{
    public Guid Id { get; set; }
    public Guid TemplateId { get; set; }
    public ReportTemplate? Template { get; set; }

    public string Name { get; set; } = string.Empty;
    public SectionType Type { get; set; }
    public double Height { get; set; } = 50; // mm
    public bool Visible { get; set; } = true;
    public string? VisibilityExpression { get; set; }
    public int OrderIndex { get; set; }

    // Elements in this section
    public List<ReportElement> Elements { get; set; } = new();
}

public enum SectionType
{
    ReportHeader,
    PageHeader,
    GroupHeader,
    Detail,
    GroupFooter,
    PageFooter,
    ReportFooter
}
