using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine;

public class Argument<T> : CliBindingSymbol<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Option{T}"/> class.
    /// </summary>
    /// <param name="id">The unique id of the option.</param>
    /// <param name="arity">The arity of the option, defaults to <see cref="Arity.ZeroOrOne"/>.</param>
    /// <param name="scope">The scope applied to this symbol.</param>
    /// <param name="converter">An object that converts a string argument value to the managed value type.</param>
    /// <param name="configureValidation">An action that configures a validation pipeline.</param>
    /// <param name="defaultProvider">
    /// function that provides a default value if the symbol is not mapped to a program argument.
    /// </param>
    /// <exception cref="InvalidOperationException">There is no default converter for <typeparamref name="T"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="id"/> is null or empty.</exception>
    /// <exception cref="FormatException"><paramref name="id"/> is not a valid identifier.</exception>
    public Argument(
        string id, 
        Arity? arity = null,
        BindingScope scope = BindingScope.Self,
        IValueConverter<T>? converter = null,
        Action<IValidationBuilder<T>>? configureValidation = null,
        Func<T>? defaultProvider = null) 
        : base(id, null, arity ?? Arity.ZeroOrOne, scope, converter, configureValidation, defaultProvider)
    {
    }

    /// <inheritdoc />
    public override CliSymbolType SymbolType => CliSymbolType.Argument;
}