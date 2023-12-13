using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine;

public sealed class Argument : Argument<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Option{T}"/> class.
    /// </summary>
    /// <param name="id">The unique id of the option.</param>
    /// <param name="arity">The arity of the option, defaults to <see cref="Arity.ZeroOrOne"/>.</param>
    /// <param name="scope">The scope applied to this symbol.</param>
    /// <param name="converter">An object that converts string argument values to the symbol value type.</param>
    /// <param name="defaultProvider">
    ///     function that provides a default value if the symbol is not mapped to a program argument.
    /// </param>
    /// <param name="validator">An object that validates binding values.</param>
    /// <exception cref="ArgumentException"><paramref name="id"/> is null or empty.</exception>
    /// <exception cref="FormatException"><paramref name="id"/> is not a valid identifier.</exception>
    public Argument(
        string id,
        Arity? arity = null,
        BindingScope scope = BindingScope.Self,
        IValueConverter<string>? converter = null,
        Func<string>? defaultProvider = null,
        IValidator<string>? validator = null) 
        : base(id, arity, scope, converter, validator, defaultProvider)
    {
    }
}