using ReportGenerator.Domain.Entities;

namespace ReportGenerator.Domain.Interfaces;

/// <summary>
/// Repository interface for data sources
/// </summary>
public interface IDataSourceRepository
{
    Task<DataSource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DataSource>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DataSource> CreateAsync(DataSource dataSource, CancellationToken cancellationToken = default);
    Task<DataSource> UpdateAsync(DataSource dataSource, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
