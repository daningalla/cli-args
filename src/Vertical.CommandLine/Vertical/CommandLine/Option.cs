using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine;

public class Option : Option<string?>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Option{T}"/> class.
    /// </summary>
    /// <param name="id">The unique id of the option.</param>
    /// <param name="aliases">An optional array of unique aliases the option is also known as.</param>
    /// <param name="arity">The arity of the option, defaults to <see cref="Arity.ZeroOrOne"/>.</param>
    /// <param name="scope">The scope applied to this symbol.</param>
    /// <param name="converter">An object that converts string argument values to the symbol value type.</param>
    /// <param name="validator">An object that ensures the value is valid within an argument context.</param>
    /// <param name="defaultProvider">
    /// function that provides a default value if the symbol is not mapped to a program argument.
    /// </param>
    /// <exception cref="ArgumentException"><paramref name="id"/> is null or empty.</exception>
    /// <exception cref="FormatException"><paramref name="id"/> is not a valid identifier.</exception>
    /// <exception cref="FormatException"><paramref name="aliases"/> contains an invalid identifier.</exception>
    public Option(
        string id, 
        string[]? aliases = null, 
        Arity? arity = null,
        BindingScope scope = BindingScope.Self,
        IValueConverter<string?>? converter = null,
        IValidator<string?>? validator = null,
        Func<string?>? defaultProvider = null) 
        : base(id, aliases, arity, scope, converter, validator, defaultProvider)
    {
    }
}