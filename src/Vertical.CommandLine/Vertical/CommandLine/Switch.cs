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
    /// <param name="validator">An object that validates binding values.</param>
    /// <exception cref="ArgumentException"><paramref name="id"/> is null or empty.</exception>
    /// <exception cref="FormatException"><paramref name="id"/> is not a valid identifier.</exception>
    /// <exception cref="FormatException"><paramref name="aliases"/> contains an invalid identifier.</exception>
    public Switch(
        string id, 
        string[]? aliases = null, 
        BindingScope scope = BindingScope.Self,
        IValueConverter<bool>? converter = null,
        IValidator<bool>? validator = null)
        : base(
            id, 
            aliases, 
            arity: Arity.ZeroOrOne, 
            scope: scope, 
            converter: converter, validator: validator, defaultProvider: () => true)
    {
    }

    /// <inheritdoc />
    public override CliSymbolType SymbolType => CliSymbolType.Switch;
}