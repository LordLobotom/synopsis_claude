using ReportGenerator.Domain.Entities;

namespace ReportGenerator.Domain.Interfaces;

/// <summary>
/// Service interface for rendering reports to various formats
/// </summary>
public interface IReportRenderer
{
    /// <summary>
    /// Renders a report to an image format
    /// </summary>
    Task<byte[]> RenderToImageAsync(ReportTemplate template, object? data, ImageFormat format, CancellationToken cancellationToken = default);

    /// <summary>
    /// Renders a report for print preview
    /// </summary>
    Task<IEnumerable<byte[]>> RenderToPagesAsync(ReportTemplate template, object? data, CancellationToken cancellationToken = default);
}

public enum ImageFormat
{
    Png,
    Jpeg
}
