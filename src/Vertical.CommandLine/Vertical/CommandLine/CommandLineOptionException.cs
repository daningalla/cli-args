using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine;

/// <summary>
/// Indicates an argument value was not provided for an operand.
/// </summary>
public class CommandLineOptionException : CommandLineException
{
    internal CommandLineOptionException(CliBindingSymbol bindingSymbol)
        : base($"{bindingSymbol.SymbolType} {bindingSymbol} requires an argument.")
    {
        BindingSymbol = bindingSymbol;
    }

    /// <summary>
    /// Gets the binding symbol.
    /// </summary>
    public CliBindingSymbol BindingSymbol { get; }
}