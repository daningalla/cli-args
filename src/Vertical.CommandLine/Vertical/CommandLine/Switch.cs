using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine;

public class Switch : Option<bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Switch"/> class.
    /// </summary>
    /// <param name="id">The unique id of the switch.</param>
    /// <param name="aliases">An optional array of unique aliases the switch is also known as.</param>
    /// <param name="scope">The scope applied to this symbol.</param>
    /// <param name="converter">An object that converts string argument values to the symbol value type.</param>
    /// <param name="configureValidator">An action that configures a validation pipeline.</param>
    /// <param name="defaultProvider">
    /// function that provides a default value if the symbol is not mapped to a program argument.
    /// </param>
    /// <exception cref="ArgumentException"><paramref name="id"/> is null or empty.</exception>
    /// <exception cref="FormatException"><paramref name="id"/> is not a valid identifier.</exception>
    /// <exception cref="FormatException"><paramref name="aliases"/> contains an invalid identifier.</exception>
    public Switch(
        string id, 
        string[]? aliases = null, 
        BindingScope scope = BindingScope.Self,
        IValueConverter<bool>? converter = null,
        Action<IValidationBuilder<bool>>? configureValidator = null,
        Func<bool>? defaultProvider = null) 
        : base(
            id, 
            aliases, 
            arity: Arity.ZeroOrOne, 
            scope, 
            converter, 
            configureValidator, 
            defaultProvider ?? (() => true))
    {
    }

    /// <inheritdoc />
    public override CliSymbolType SymbolType => CliSymbolType.Switch;
}