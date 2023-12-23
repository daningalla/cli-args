using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine;

/// <summary>
/// Indicates an argument value failed validation.
/// </summary>
public class CommandLineValidationException : CommandLineException
{
    private CommandLineValidationException(
        string message, 
        CliBindingSymbol bindingSymbol, 
        object attemptedValue)
        : base(message)
    {
        BindingSymbol = bindingSymbol;
        AttemptedValue = attemptedValue;
    }

    /// <summary>
    /// Gets the binding symbol.
    /// </summary>
    public CliBindingSymbol BindingSymbol { get; }

    /// <summary>
    /// Gets the attempted value.
    /// </summary>
    public object AttemptedValue { get; }

    internal static CommandLineValidationException Create<T>(ValidationContext<T> context)
    {
        var constraint = context.Failures.FirstOrDefault();
        var messageClause = constraint?.MessageFormatter?.Invoke(context.AttemptedValue)
                            ?? $"Attempted value \"{context.AttemptedValue}\" is invalid.";
        var message = $"{context.Symbol.SymbolType} {context.Symbol}: {messageClause}";

        return new CommandLineValidationException(
            message,
            context.Symbol,
            context.AttemptedValue!);

    }
}