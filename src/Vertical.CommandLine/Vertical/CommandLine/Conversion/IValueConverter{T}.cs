namespace Vertical.CommandLine.Conversion;

/// <summary>
/// Represents an object that converts string to a specific type.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public interface IValueConverter<T> : IValueConverter
{
    /// <summary>
    /// When implemented by a class, converts a string value to a <typeparamref name="T"/> value.
    /// </summary>
    /// <param name="context">Contains information about the conversion operation.</param>
    /// <returns>The value of <typeparamref name="T"/>.</returns>
    T Convert(ConversionContext<T> context);
}