using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Binding;

public sealed class SingleArgumentValueBinding<T> : IArgumentValueBinding<T>
{
    internal SingleArgumentValueBinding(CliBindingSymbol<T> symbol, string? argumentValue)
    {
        ArgumentNullException.ThrowIfNull(symbol);

        ArgumentValue = argumentValue;
        Symbol = symbol;
    }

    /// <inheritdoc />
    public string BindingId => Symbol.Id;

    /// <inheritdoc />
    public string ParameterId => NamingUtilities.GetInferredBindingName(Symbol.Id);

    /// <summary>
    /// Gets the argument value or <c>null</c> if an argument was not provided.
    /// </summary>
    public string? ArgumentValue { get; }

    /// <inheritdoc />
    public CliSymbol BaseSymbol => Symbol;

    /// <summary>
    /// Gets the binding symbol.
    /// </summary>
    public CliBindingSymbol<T> Symbol { get; }

    /// <inheritdoc />
    public Type ValueType => typeof(T);

    /// <inheritdoc />
    public override string ToString() => ArgumentValue != null ? $"{Symbol.Id}=\"{ArgumentValue}\"" : "(null)";
}