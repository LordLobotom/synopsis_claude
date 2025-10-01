using NCalc;
using ReportGenerator.Domain.Interfaces;

namespace ReportGenerator.Infrastructure.Services;

/// <summary>
/// Expression evaluator service using NCalc library
/// </summary>
public class ExpressionEvaluatorService : IExpressionEvaluator
{
    public object? Evaluate(string expression, IDictionary<string, object?> context)
    {
        try
        {
            var expr = new Expression(expression, EvaluateOptions.IgnoreCase);

            // Add context parameters
            foreach (var kvp in context)
            {
                expr.Parameters[kvp.Key] = kvp.Value;
            }

            // Add custom functions
            RegisterCustomFunctions(expr);

            return expr.Evaluate();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error evaluating expression: {expression}", ex);
        }
    }

    public bool ValidateExpression(string expression, out string? errorMessage)
    {
        try
        {
            var expr = new Expression(expression);
            _ = expr.HasErrors();
            errorMessage = expr.Error;
            return !expr.HasErrors();
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            return false;
        }
    }

    public IEnumerable<string> GetAvailableFunctions()
    {
        return new[]
        {
            // Mathematical
            "ABS", "CEILING", "FLOOR", "ROUND", "SQRT", "POWER", "EXP", "LOG", "LOG10",
            "SIN", "COS", "TAN", "ASIN", "ACOS", "ATAN",

            // String
            "LEFT", "RIGHT", "MID", "UPPER", "LOWER", "TRIM", "LTRIM", "RTRIM",
            "LEN", "REPLACE", "SUBSTRING", "CONTAINS", "STARTSWITH", "ENDSWITH",
            "CONCAT", "FORMAT", "PADLEFT", "PADRIGHT", "REVERSE", "INDEXOF",

            // Date/Time
            "NOW", "TODAY", "YEAR", "MONTH", "DAY", "HOUR", "MINUTE", "SECOND",
            "DATEADD", "DATEDIFF", "DATEFORMAT", "TIMEFORMAT", "WEEKDAY", "WEEKNUM", "QUARTER",

            // Conditional
            "IF", "ISNULL", "ISEMPTY", "COALESCE", "NULLIF",

            // Conversion
            "TOSTRING", "TONUMBER", "TODATE", "TOBOOLEAN", "CAST"
        };
    }

    private void RegisterCustomFunctions(Expression expr)
    {
        // String functions
        expr.EvaluateFunction += (name, args) =>
        {
            switch (name.ToUpper())
            {
                case "LEFT":
                    if (args.Parameters.Length >= 2)
                    {
                        var str = args.Parameters[0].Evaluate()?.ToString() ?? "";
                        var len = Convert.ToInt32(args.Parameters[1].Evaluate());
                        args.Result = str.Length <= len ? str : str.Substring(0, len);
                    }
                    break;

                case "RIGHT":
                    if (args.Parameters.Length >= 2)
                    {
                        var str = args.Parameters[0].Evaluate()?.ToString() ?? "";
                        var len = Convert.ToInt32(args.Parameters[1].Evaluate());
                        args.Result = str.Length <= len ? str : str.Substring(str.Length - len);
                    }
                    break;

                case "MID":
                case "SUBSTRING":
                    if (args.Parameters.Length >= 3)
                    {
                        var str = args.Parameters[0].Evaluate()?.ToString() ?? "";
                        var start = Convert.ToInt32(args.Parameters[1].Evaluate());
                        var len = Convert.ToInt32(args.Parameters[2].Evaluate());
                        args.Result = str.Substring(start, Math.Min(len, str.Length - start));
                    }
                    break;

                case "UPPER":
                    args.Result = args.Parameters[0].Evaluate()?.ToString()?.ToUpper() ?? "";
                    break;

                case "LOWER":
                    args.Result = args.Parameters[0].Evaluate()?.ToString()?.ToLower() ?? "";
                    break;

                case "TRIM":
                    args.Result = args.Parameters[0].Evaluate()?.ToString()?.Trim() ?? "";
                    break;

                case "LEN":
                    args.Result = args.Parameters[0].Evaluate()?.ToString()?.Length ?? 0;
                    break;

                case "REPLACE":
                    if (args.Parameters.Length >= 3)
                    {
                        var str = args.Parameters[0].Evaluate()?.ToString() ?? "";
                        var oldVal = args.Parameters[1].Evaluate()?.ToString() ?? "";
                        var newVal = args.Parameters[2].Evaluate()?.ToString() ?? "";
                        args.Result = str.Replace(oldVal, newVal);
                    }
                    break;

                case "CONCAT":
                    args.Result = string.Concat(args.Parameters.Select(p => p.Evaluate()?.ToString() ?? ""));
                    break;

                case "NOW":
                    args.Result = DateTime.Now;
                    break;

                case "TODAY":
                    args.Result = DateTime.Today;
                    break;

                case "YEAR":
                    args.Result = Convert.ToDateTime(args.Parameters[0].Evaluate()).Year;
                    break;

                case "MONTH":
                    args.Result = Convert.ToDateTime(args.Parameters[0].Evaluate()).Month;
                    break;

                case "DAY":
                    args.Result = Convert.ToDateTime(args.Parameters[0].Evaluate()).Day;
                    break;

                case "IF":
                    if (args.Parameters.Length >= 3)
                    {
                        var condition = Convert.ToBoolean(args.Parameters[0].Evaluate());
                        args.Result = condition ? args.Parameters[1].Evaluate() : args.Parameters[2].Evaluate();
                    }
                    break;

                case "ISNULL":
                    if (args.Parameters.Length >= 2)
                    {
                        var val = args.Parameters[0].Evaluate();
                        args.Result = val ?? args.Parameters[1].Evaluate();
                    }
                    break;

                case "ISEMPTY":
                    var value = args.Parameters[0].Evaluate();
                    args.Result = value == null || string.IsNullOrWhiteSpace(value.ToString());
                    break;

                case "FORMAT":
                    if (args.Parameters.Length >= 2)
                    {
                        var format = args.Parameters[0].Evaluate()?.ToString() ?? "";
                        var val = args.Parameters[1].Evaluate();
                        args.Result = string.Format(format, val);
                    }
                    break;

                case "TOSTRING":
                    args.Result = args.Parameters[0].Evaluate()?.ToString() ?? "";
                    break;

                case "TONUMBER":
                    args.Result = Convert.ToDouble(args.Parameters[0].Evaluate());
                    break;
            }
        };
    }
}
