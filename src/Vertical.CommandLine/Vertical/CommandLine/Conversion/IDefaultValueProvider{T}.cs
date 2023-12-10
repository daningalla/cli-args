namespace Vertical.CommandLine.Conversion;

/// <summary>
/// Represents an object that returns the default value for a specific type.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public interface IDefaultValueProvider<out T> : IDefaultValueProvider
{
    /// <summary>
    /// Gets the default value provided.
    /// </summary>
    T Value { get; }
}