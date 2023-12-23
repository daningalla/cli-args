using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine;

/// <summary>
/// Indicates a condition in which conversion to a binding symbol type failed.
/// </summary>
public sealed class CommandLineConversionException : CommandLineException
{
    internal CommandLineConversionException(
        CliBindingSymbol bindingSymbol,
        string attemptedValue,
        Exception innerException)
        : base(FormatMessage(bindingSymbol, attemptedValue), innerException)
    {
        BindingSymbol = bindingSymbol;
        AttemptedValue = attemptedValue;
    }

    /// <summary>
    /// Gets the option or argument binding symbol.
    /// </summary>
    public CliBindingSymbol BindingSymbol { get; }

    /// <summary>
    /// Gets the attempted value.
    /// </summary>
    public string AttemptedValue { get; }

    private static string FormatMessage(CliBindingSymbol bindingSymbol, string attemptedValue)
    {
        return $"{bindingSymbol.SymbolType} '{bindingSymbol}': Cannot convert argument \"{attemptedValue}\" " +
                      "to {bindingSymbol.ValueType}.";
    }
}