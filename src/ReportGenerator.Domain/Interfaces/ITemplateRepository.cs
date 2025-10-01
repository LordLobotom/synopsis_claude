using ReportGenerator.Domain.Entities;

namespace ReportGenerator.Domain.Interfaces;

/// <summary>
/// Repository interface for report templates
/// </summary>
public interface ITemplateRepository
{
    Task<ReportTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReportTemplate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ReportTemplate> CreateAsync(ReportTemplate template, CancellationToken cancellationToken = default);
    Task<ReportTemplate> UpdateAsync(ReportTemplate template, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReportTemplate>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
}
