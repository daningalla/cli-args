namespace Vertical.CommandLine.Conversion;

internal sealed class EnumConverter<T> : IValueConverter<T>
{
    /// <inheritdoc />
    public T Convert(ConversionContext<T> context)
    {
        return (T)Enum.Parse(typeof(T), context.Value, ignoreCase: true);
    }

    /// <inheritdoc />
    public Type ServiceType => typeof(IValueConverter<T>);

    /// <inheritdoc />
    public Type ValueType => typeof(T);
}