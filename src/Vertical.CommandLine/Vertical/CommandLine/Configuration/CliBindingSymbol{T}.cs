using Vertical.CommandLine.Binding;
using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine.Configuration;

public abstract class CliBindingSymbol<T> : CliBindingSymbol
{
    private protected CliBindingSymbol(
        string id, 
        string[]? aliases, 
        Arity arity,
        BindingScope scope,
        IValueConverter<T>? converter,
        IValidator<T>? validator,
        Func<T>? defaultProvider) 
        : base(id, aliases, arity, scope, typeof(T))
    {
        DefaultProvider = defaultProvider != null ? new DefaultValueProvider<T>(defaultProvider) : null;
        Converter = converter;
        Validator = validator;
    }

    /// <summary>
    /// Gets the object responsible for converting string argument values to the expected type.
    /// </summary>
    public IValueConverter<T>? Converter { get; }

    /// <summary>
    /// Gets the object responsible for validating values.
    /// </summary>
    public IValidator<T>? Validator { get; }

    /// <summary>
    /// Gets a function that provides a default value if the symbol is not mapped to
    /// an argument.
    /// </summary>
    public IDefaultValueProvider<T>? DefaultProvider { get; }

    /// <inheritdoc />
    public override IArgumentValueBinding CreateBinding(string? value) =>
        new SingleArgumentValueBinding<T>(this, value);

    /// <inheritdoc />
    public override IArgumentValueBinding CreateMultiValueBinding(IEnumerable<string> values) =>
        new MultiValueArgumentBinding<T>(this, values);

    /// <inheritdoc />
    public override bool HasValueConverter => Converter != null;

    /// <inheritdoc />
    public override string ToString() => string.Join(" | ", Identifiers);
}