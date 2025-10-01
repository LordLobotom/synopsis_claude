using Microsoft.EntityFrameworkCore;
using ReportGenerator.Domain.Entities;

namespace ReportGenerator.Infrastructure.Data;

/// <summary>
/// EF Core DbContext for template storage (SQLite)
/// </summary>
public class ReportDbContext : DbContext
{
    public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options)
    {
    }

    public DbSet<ReportTemplate> Templates { get; set; } = null!;
    public DbSet<ReportSection> Sections { get; set; } = null!;
    public DbSet<ReportElement> Elements { get; set; } = null!;
    public DbSet<DataSource> DataSources { get; set; } = null!;
    public DbSet<DataSourceParameter> DataSourceParameters { get; set; } = null!;
    public DbSet<ConnectionString> ConnectionStrings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ReportTemplate configuration
        modelBuilder.Entity<ReportTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.OwnsOne(e => e.Margins);
            entity.HasMany(e => e.Sections)
                .WithOne(s => s.Template)
                .HasForeignKey(s => s.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.Name);
        });

        // ReportSection configuration
        modelBuilder.Entity<ReportSection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasMany(e => e.Elements)
                .WithOne(e => e.Section)
                .HasForeignKey(e => e.SectionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ReportElement configuration
        modelBuilder.Entity<ReportElement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FontFamily).HasMaxLength(50);
            entity.Property(e => e.ForeColor).HasMaxLength(20);
            entity.Property(e => e.BackColor).HasMaxLength(20);
            entity.Property(e => e.BorderColor).HasMaxLength(20);
            entity.Property(e => e.Properties).HasColumnType("TEXT"); // JSON
        });

        // DataSource configuration
        modelBuilder.Entity<DataSource>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasMany(e => e.Parameters)
                .WithOne()
                .HasForeignKey(p => p.DataSourceId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.Name);
        });

        // DataSourceParameter configuration
        modelBuilder.Entity<DataSourceParameter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DataType).HasMaxLength(50);
        });

        // ConnectionString configuration
        modelBuilder.Entity<ConnectionString>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Server).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Database).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Username).HasMaxLength(100);
            entity.Property(e => e.EncryptedPassword).HasMaxLength(500);
            entity.HasIndex(e => e.Name);
        });
    }
}
