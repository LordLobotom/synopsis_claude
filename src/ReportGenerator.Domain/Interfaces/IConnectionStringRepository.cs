using ReportGenerator.Domain.Entities;

namespace ReportGenerator.Domain.Interfaces;

/// <summary>
/// Repository interface for connection strings
/// </summary>
public interface IConnectionStringRepository
{
    Task<ConnectionString?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ConnectionString>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ConnectionString> CreateAsync(ConnectionString connectionString, CancellationToken cancellationToken = default);
    Task<ConnectionString> UpdateAsync(ConnectionString connectionString, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
