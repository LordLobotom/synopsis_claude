using Microsoft.EntityFrameworkCore;
using ReportGenerator.Domain.Entities;
using ReportGenerator.Domain.Interfaces;
using ReportGenerator.Infrastructure.Data;

namespace ReportGenerator.Infrastructure.Repositories;

public class ConnectionStringRepository : IConnectionStringRepository
{
    private readonly ReportDbContext _context;

    public ConnectionStringRepository(ReportDbContext context)
    {
        _context = context;
    }

    public async Task<ConnectionString?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ConnectionStrings.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<ConnectionString>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ConnectionStrings
            .OrderBy(cs => cs.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<ConnectionString> CreateAsync(ConnectionString connectionString, CancellationToken cancellationToken = default)
    {
        connectionString.Id = Guid.NewGuid();
        connectionString.CreatedAt = DateTime.UtcNow;
        connectionString.ModifiedAt = DateTime.UtcNow;

        _context.ConnectionStrings.Add(connectionString);
        await _context.SaveChangesAsync(cancellationToken);
        return connectionString;
    }

    public async Task<ConnectionString> UpdateAsync(ConnectionString connectionString, CancellationToken cancellationToken = default)
    {
        connectionString.ModifiedAt = DateTime.UtcNow;

        _context.ConnectionStrings.Update(connectionString);
        await _context.SaveChangesAsync(cancellationToken);
        return connectionString;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var connectionString = await _context.ConnectionStrings.FindAsync(new object[] { id }, cancellationToken);
        if (connectionString != null)
        {
            _context.ConnectionStrings.Remove(connectionString);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
