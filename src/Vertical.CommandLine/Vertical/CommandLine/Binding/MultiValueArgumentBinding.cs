using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Binding;

public sealed class MultiValueArgumentBinding<T> : IArgumentValueBinding<T>
{
    internal MultiValueArgumentBinding(CliBindingSymbol<T> bindingSymbol, IEnumerable<string> argumentValues)
    {
        ArgumentNullException.ThrowIfNull(argumentValues);
        ArgumentNullException.ThrowIfNull(bindingSymbol);

        ArgumentValues = argumentValues;
        Symbol = bindingSymbol;
    }

    /// <inheritdoc />
    public string BindingId => Symbol.Id;

    /// <inheritdoc />
    public string ParameterId => NamingUtilities.GetInferredBindingName(Symbol.Id);

    /// <summary>
    /// Gets the arguments specified by the application.
    /// </summary>
    public IEnumerable<string> ArgumentValues { get; }

    /// <inheritdoc />
    public CliSymbol BaseSymbol => Symbol;

    /// <summary>
    /// Gets the binding symbol.
    /// </summary>
    public CliBindingSymbol<T> Symbol { get; }

    /// <inheritdoc />
    public Type ValueType => typeof(T);

    /// <inheritdoc />
    public override string ToString()
    {
        var values = string.Join(',', ArgumentValues.Select(str => $"\"{str}\""));
        return $"{Symbol.Id}=[{values}]";
    }
}