namespace Vertical.CommandLine.Binding;

/// <summary>
/// Represents a strongly typed wrapper of a value.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public interface IModelValue<out T> : IModelValue
{
    /// <summary>
    /// Gets the wrapped model value.
    /// </summary>
    T Value { get; }
}