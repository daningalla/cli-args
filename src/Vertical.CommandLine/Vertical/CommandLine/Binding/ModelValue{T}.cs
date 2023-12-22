using CommunityToolkit.Diagnostics;

namespace Vertical.CommandLine.Binding;

/// <summary>
/// Wraps the value of a bound model.
/// </summary>
/// <typeparam name="T">Model type.</typeparam>
public sealed class ModelValue<T> : IModelValue<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModelValue{T}"/> class.
    /// </summary>
    /// <param name="value">The model value.</param>
    public ModelValue(T value)
    {
        Guard.IsNotNull(value);
        Value = value;
    }
    
    /// <inheritdoc />
    public Type ServiceType => typeof(IModelValue<T>);

    /// <inheritdoc />
    public T Value { get; }
}