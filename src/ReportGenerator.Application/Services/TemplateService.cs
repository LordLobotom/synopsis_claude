using ReportGenerator.Domain.Entities;
using ReportGenerator.Domain.Interfaces;

namespace ReportGenerator.Application.Services;

/// <summary>
/// Application service for template management
/// </summary>
public class TemplateService : ITemplateService
{
    private readonly ITemplateRepository _templateRepository;

    public TemplateService(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<ReportTemplate?> GetTemplateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _templateRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<ReportTemplate>> GetAllTemplatesAsync(CancellationToken cancellationToken = default)
    {
        return await _templateRepository.GetAllAsync(cancellationToken);
    }

    public async Task<ReportTemplate> CreateTemplateAsync(string name, string description, string author, CancellationToken cancellationToken = default)
    {
        var template = new ReportTemplate
        {
            Name = name,
            Description = description,
            Author = author,
            Orientation = PageOrientation.Portrait,
            PaperSize = PaperSize.A4,
            Margins = new Margin()
        };

        // Add default sections
        template.Sections.Add(new ReportSection
        {
            Id = Guid.NewGuid(),
            Name = "Page Header",
            Type = SectionType.PageHeader,
            Height = 30,
            OrderIndex = 0
        });

        template.Sections.Add(new ReportSection
        {
            Id = Guid.NewGuid(),
            Name = "Detail",
            Type = SectionType.Detail,
            Height = 50,
            OrderIndex = 1
        });

        template.Sections.Add(new ReportSection
        {
            Id = Guid.NewGuid(),
            Name = "Page Footer",
            Type = SectionType.PageFooter,
            Height = 30,
            OrderIndex = 2
        });

        return await _templateRepository.CreateAsync(template, cancellationToken);
    }

    public async Task<ReportTemplate> UpdateTemplateAsync(ReportTemplate template, CancellationToken cancellationToken = default)
    {
        return await _templateRepository.UpdateAsync(template, cancellationToken);
    }

    public async Task DeleteTemplateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _templateRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<ReportTemplate>> SearchTemplatesAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _templateRepository.SearchByNameAsync(searchTerm, cancellationToken);
    }
}
