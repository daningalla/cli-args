namespace Vertical.CommandLine.Conversion;

/// <summary>
/// Represents an object that converts string argument values using a delegate.
/// </summary>
/// <typeparam name="T">Conversion target type.</typeparam>
public sealed class ValueConverter<T> : IValueConverter<T>
{
    private readonly Func<string, T> _converter;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ValueConverter{T}"/> class.
    /// </summary>
    /// <param name="converter">The function that implements conversion.</param>
    public ValueConverter(Func<string, T> converter)
    {
        ArgumentNullException.ThrowIfNull(converter);
        _converter = converter;
    }

    /// <inheritdoc />
    public T Convert(ConversionContext<T> context) => _converter(context.Value);

    /// <inheritdoc />
    public Type ServiceType => typeof(IValueConverter<T>);

    /// <inheritdoc />
    public Type ValueType => typeof(T);
}