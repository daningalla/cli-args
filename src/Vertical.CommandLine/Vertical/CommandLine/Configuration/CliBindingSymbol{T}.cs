﻿using Vertical.CommandLine.Binding;
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
    public override IArgumentValueBindingFactory CreateBindingFactory() => new ArgumentValueBindingFactory<T>();

    /// <inheritdoc />
    public override IValueConverter? GetDefaultConverter() => DefaultValueConverter<T>.Instance;

    /// <inheritdoc />
    public override bool HasConverter => Converter is not null;

    /// <inheritdoc />
    public override string ToString() => string.Join(" | ", Identifiers);
}