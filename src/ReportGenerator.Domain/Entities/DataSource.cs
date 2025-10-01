namespace ReportGenerator.Domain.Entities;

/// <summary>
/// Represents a data source configuration for reports
/// </summary>
public class DataSource
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DataSourceType Type { get; set; }

    // Connection
    public Guid? ConnectionStringId { get; set; }
    public ConnectionString? ConnectionString { get; set; }

    // Query
    public string? SqlQuery { get; set; }
    public string? StoredProcedureName { get; set; }
    public string? TableName { get; set; }

    // Parameters
    public List<DataSourceParameter> Parameters { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}

public class DataSourceParameter
{
    public Guid Id { get; set; }
    public Guid DataSourceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = "string";
    public string? DefaultValue { get; set; }
    public bool Required { get; set; }
}

public enum DataSourceType
{
    SqlQuery,
    StoredProcedure,
    Table
}
