using CommunityToolkit.Diagnostics;
using Vertical.CommandLine.Binding;
using Vertical.CommandLine.Conversion;

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
        Guard.IsNotNullOrWhiteSpace(id);
        
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
    /// Creates an argument value binding factory.
    /// </summary>
    /// <returns><see cref="IArgumentValueBindingFactory"/></returns>
    public abstract IArgumentValueBindingFactory CreateBindingFactory();

    /// <summary>
    /// Gets the default <see cref="IValueConverter"/> instance for this symbol.
    /// </summary>
    /// <returns><see cref="IValueConverter"/> or <c>null</c> if a default converter is not available.</returns>
    public abstract IValueConverter? GetDefaultConverter();
    
    /// <summary>
    /// Gets whether the symbol defines a value converter.
    /// </summary>
    public abstract bool HasConverter { get; }
}