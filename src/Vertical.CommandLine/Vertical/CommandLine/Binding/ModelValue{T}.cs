namespace Vertical.CommandLine.Binding;

public sealed class ModelValue<T> : IModelValue<T>
{
    public ModelValue(T value)
    {
        Value = value;
    }
    
    /// <inheritdoc />
    public Type ServiceType => typeof(IModelValue<T>);

    /// <inheritdoc />
    public T Value { get; }
}