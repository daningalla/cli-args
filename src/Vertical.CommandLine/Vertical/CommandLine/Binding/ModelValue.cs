namespace Vertical.CommandLine.Binding;

internal sealed class ModelValue<T> : IModelValue<T>
{
    internal ModelValue(T value)
    {
        Value = value;
    }
    
    /// <inheritdoc />
    public Type ServiceType => typeof(IModelValue<T>);

    /// <inheritdoc />
    public T Value { get; }
}