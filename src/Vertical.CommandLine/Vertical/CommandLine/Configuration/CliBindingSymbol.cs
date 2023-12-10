using Vertical.CommandLine.Binding;

namespace Vertical.CommandLine.Configuration;

public abstract class CliBindingSymbol : CliSymbol
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CliBindingSymbol"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the symbol.</param>
    /// <param name="aliases">Optional aliases the symbol is also known as.</param>
    /// <param name="arity">The arity of use expected of the symbol.</param>
    /// <param name="scope">The scope applied to this symbol.</param>
    /// <param name="valueType">The value type represented by the symbol.</param>
    private protected CliBindingSymbol(
        string id, 
        string[]? aliases,
        Arity arity,
        BindingScope scope,
        Type valueType) 
        : base(id, aliases)
    {
        Arity = arity;
        Scope = scope;
        ValueType = valueType;
    }

    /// <summary>
    /// Gets the <see cref="Arity"/> use requirement for the symbol.
    /// </summary>
    public Arity Arity { get; }

    /// <summary>
    /// Gets the value type managed by the symbol.
    /// </summary>
    public Type ValueType { get; }
    
    /// <summary>
    /// Gets the binding scope.
    /// </summary>
    public BindingScope Scope { get; }

    /// <summary>
    /// Creates a single value argument binding.
    /// </summary>
    /// <param name="value">Binding value.</param>
    /// <returns><see cref="IArgumentValueBinding"/></returns>
    public abstract IArgumentValueBinding CreateBinding(string? value);

    /// <summary>
    /// Creates a multi value argument binding.
    /// </summary>
    /// <param name="values">Binding value.</param>
    /// <returns><see cref="IArgumentValueBinding"/></returns>
    public abstract IArgumentValueBinding CreateMultiValueBinding(IEnumerable<string> values);
    
    /// <summary>
    /// Gets whether the binding symbol has a defined value converter.
    /// </summary>
    public abstract bool HasValueConverter { get; }
}