﻿using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine;

public class Option<T> : CliBindingSymbol<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Option{T}"/> class.
    /// </summary>
    /// <param name="id">The unique id of the option.</param>
    /// <param name="aliases">An optional array of unique aliases the option is also known as.</param>
    /// <param name="arity">The arity of the option, defaults to <see cref="Arity.ZeroOrOne"/>.</param>
    /// <param name="scope">The scope applied to this symbol.</param>
    /// <param name="converter">An object that converts a string argument value to the managed value type.</param>
    /// <param name="validator">An object that validates the binding value.</param>
    /// <param name="defaultProvider">
    ///     function that provides a default value if the symbol is not mapped to a program argument.
    /// </param>
    /// <exception cref="InvalidOperationException">There is no default converter for <typeparamref name="T"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="id"/> is null or empty.</exception>
    /// <exception cref="FormatException"><paramref name="id"/> is not a valid identifier.</exception>
    /// <exception cref="FormatException"><paramref name="aliases"/> contains an invalid identifier.</exception>
    public Option(
        string id,
        string[]? aliases = null,
        Arity? arity = null,
        BindingScope scope = BindingScope.Self,
        IValueConverter<T>? converter = null,
        IValidator<T>? validator = null,
        Func<T>? defaultProvider = null)
        : base(
            id, aliases, arity ?? Arity.ZeroOrOne, scope, 
            converter, 
            validator, 
            defaultProvider)
    {
    }

    /// <inheritdoc />
    public override CliSymbolType SymbolType => CliSymbolType.Option;
}