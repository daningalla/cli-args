namespace Vertical.CommandLine.Conversion;

/// <summary>
/// Base class for value converters.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public abstract class ValueConverter<T> : IValueConverter<T>
{
    /// <inheritdoc />
    public abstract T Convert(ConversionContext<T> context);

    /// <inheritdoc />
    public Type ServiceType => typeof(IValueConverter<T>);

    /// <inheritdoc />
    public Type ValueType => typeof(T);
}