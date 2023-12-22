using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Binding;

internal sealed class ArgumentValueBinding<T> : IArgumentValueBinding<T>
{
    internal ArgumentValueBinding(CliBindingSymbol<T> symbol, T[] argumentValues)
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

    /// <inheritdoc />
    public T[] ArgumentValues { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        var valueString = ArgumentValues switch
        {
            { Length: 0 } => "(none)",
            { Length: 1 } => $"{ArgumentValues[0]}",
            _ => $"[{string.Join(',', ArgumentValues)}]"
        };
        return $"{Symbol.Id} = {valueString}";
    }
}