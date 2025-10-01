namespace ReportGenerator.Domain.Entities;

/// <summary>
/// Represents a database connection string configuration
/// </summary>
public class ConnectionString
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DatabaseProvider Provider { get; set; }
    public string Server { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? EncryptedPassword { get; set; }
    public bool UseWindowsAuth { get; set; }
    public int Port { get; set; }
    public string? AdditionalParameters { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    /// <summary>
    /// Builds the connection string (password decryption handled elsewhere)
    /// </summary>
    public string BuildConnectionString(string? decryptedPassword = null)
    {
        return Provider switch
        {
            DatabaseProvider.SqlServer => BuildSqlServerConnectionString(decryptedPassword),
            DatabaseProvider.Sqlite => $"Data Source={Database}",
            _ => throw new NotSupportedException($"Provider {Provider} not supported")
        };
    }

    private string BuildSqlServerConnectionString(string? decryptedPassword)
    {
        if (UseWindowsAuth)
        {
            return $"Server={Server};Database={Database};Integrated Security=true;{AdditionalParameters}";
        }
        else
        {
            var password = decryptedPassword ?? string.Empty;
            return $"Server={Server};Database={Database};User Id={Username};Password={password};{AdditionalParameters}";
        }
    }
}

public enum DatabaseProvider
{
    SqlServer,
    Sqlite
}
