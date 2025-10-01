namespace ReportGenerator.Domain.Interfaces;

/// <summary>
/// Service interface for evaluating expressions in reports
/// </summary>
public interface IExpressionEvaluator
{
    /// <summary>
    /// Evaluates an expression with the given data context
    /// </summary>
    object? Evaluate(string expression, IDictionary<string, object?> context);

    /// <summary>
    /// Validates an expression syntax
    /// </summary>
    bool ValidateExpression(string expression, out string? errorMessage);

    /// <summary>
    /// Gets list of available functions
    /// </summary>
    IEnumerable<string> GetAvailableFunctions();
}
