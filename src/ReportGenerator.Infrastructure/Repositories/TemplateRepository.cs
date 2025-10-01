using Microsoft.EntityFrameworkCore;
using ReportGenerator.Domain.Entities;
using ReportGenerator.Domain.Interfaces;
using ReportGenerator.Infrastructure.Data;

namespace ReportGenerator.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for report templates
/// </summary>
public class TemplateRepository : ITemplateRepository
{
    private readonly ReportDbContext _context;

    public TemplateRepository(ReportDbContext context)
    {
        _context = context;
    }

    public async Task<ReportTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Templates
            .Include(t => t.Sections.OrderBy(s => s.OrderIndex))
                .ThenInclude(s => s.Elements.OrderBy(e => e.ZIndex))
            .Include(t => t.DataSource)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ReportTemplate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Templates
            .OrderByDescending(t => t.ModifiedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<ReportTemplate> CreateAsync(ReportTemplate template, CancellationToken cancellationToken = default)
    {
        template.Id = Guid.NewGuid();
        template.CreatedAt = DateTime.UtcNow;
        template.ModifiedAt = DateTime.UtcNow;
        template.Version = 1;

        _context.Templates.Add(template);
        await _context.SaveChangesAsync(cancellationToken);
        return template;
    }

    public async Task<ReportTemplate> UpdateAsync(ReportTemplate template, CancellationToken cancellationToken = default)
    {
        template.ModifiedAt = DateTime.UtcNow;
        template.Version++;

        _context.Templates.Update(template);
        await _context.SaveChangesAsync(cancellationToken);
        return template;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var template = await _context.Templates.FindAsync(new object[] { id }, cancellationToken);
        if (template != null)
        {
            _context.Templates.Remove(template);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<ReportTemplate>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Templates
            .Where(t => t.Name.Contains(searchTerm) || t.Description.Contains(searchTerm))
            .OrderByDescending(t => t.ModifiedAt)
            .ToListAsync(cancellationToken);
    }
}
