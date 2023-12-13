using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Binding;

public class ArgumentValueBinding<T> : IArgumentValueBinding<T>
{
    internal ArgumentValueBinding(CliBindingSymbol<T> symbol, IEnumerable<string> argumentValues)
    {
        Symbol = symbol;
        ArgumentValues = argumentValues;
    }

    /// <inheritdoc />
    public string BindingId => Symbol.Id;

    /// <inheritdoc />
    public Type ValueType => Symbol.ValueType;

    /// <inheritdoc />
    public CliSymbol BaseSymbol => Symbol;

    /// <inheritdoc />
    public CliBindingSymbol<T> Symbol { get; }

    public IEnumerable<string> ArgumentValues { get; }
}