using Microsoft.EntityFrameworkCore;
using ReportGenerator.Domain.Entities;
using ReportGenerator.Domain.Interfaces;
using ReportGenerator.Infrastructure.Data;

namespace ReportGenerator.Infrastructure.Repositories;

public class DataSourceRepository : IDataSourceRepository
{
    private readonly ReportDbContext _context;

    public DataSourceRepository(ReportDbContext context)
    {
        _context = context;
    }

    public async Task<DataSource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DataSources
            .Include(ds => ds.Parameters)
            .Include(ds => ds.ConnectionString)
            .FirstOrDefaultAsync(ds => ds.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DataSource>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DataSources
            .Include(ds => ds.ConnectionString)
            .ToListAsync(cancellationToken);
    }

    public async Task<DataSource> CreateAsync(DataSource dataSource, CancellationToken cancellationToken = default)
    {
        dataSource.Id = Guid.NewGuid();
        dataSource.CreatedAt = DateTime.UtcNow;
        dataSource.ModifiedAt = DateTime.UtcNow;

        _context.DataSources.Add(dataSource);
        await _context.SaveChangesAsync(cancellationToken);
        return dataSource;
    }

    public async Task<DataSource> UpdateAsync(DataSource dataSource, CancellationToken cancellationToken = default)
    {
        dataSource.ModifiedAt = DateTime.UtcNow;

        _context.DataSources.Update(dataSource);
        await _context.SaveChangesAsync(cancellationToken);
        return dataSource;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dataSource = await _context.DataSources.FindAsync(new object[] { id }, cancellationToken);
        if (dataSource != null)
        {
            _context.DataSources.Remove(dataSource);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
