using ReportGenerator.Domain.Entities;

namespace ReportGenerator.Application.Services;

/// <summary>
/// Application service interface for template management
/// </summary>
public interface ITemplateService
{
    Task<ReportTemplate?> GetTemplateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReportTemplate>> GetAllTemplatesAsync(CancellationToken cancellationToken = default);
    Task<ReportTemplate> CreateTemplateAsync(string name, string description, string author, CancellationToken cancellationToken = default);
    Task<ReportTemplate> UpdateTemplateAsync(ReportTemplate template, CancellationToken cancellationToken = default);
    Task DeleteTemplateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReportTemplate>> SearchTemplatesAsync(string searchTerm, CancellationToken cancellationToken = default);
}
