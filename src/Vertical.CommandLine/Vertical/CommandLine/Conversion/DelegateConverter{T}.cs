namespace Vertical.CommandLine.Conversion;

/// <summary>
/// Represents an object that converts string argument values using a delegate.
/// </summary>
/// <typeparam name="T">Conversion target type.</typeparam>
public sealed class DelegateConverter<T> : ValueConverter<T>
{
    private readonly Func<string, T> _converter;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateConverter{T}"/> class.
    /// </summary>
    /// <param name="converter">The function that implements conversion.</param>
    public DelegateConverter(Func<string, T> converter)
    {
        ArgumentNullException.ThrowIfNull(converter);
        _converter = converter;
    }

    /// <inheritdoc />
    public override T Convert(ConversionContext<T> context) => _converter(context.Value);
}