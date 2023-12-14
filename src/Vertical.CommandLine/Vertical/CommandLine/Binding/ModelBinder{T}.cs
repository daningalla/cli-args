using Vertical.CommandLine.Invocation;

namespace Vertical.CommandLine.Binding;

public abstract class ModelBinder<T> : IModelBinder
{
    /// <inheritdoc />
    public Type ValueType => typeof(T);

    /// <inheritdoc />
    public Type ServiceType => typeof(IModelBinder);

    /// <inheritdoc />
    public IModelValue BindInstanceBase(IMappedArgumentProvider argumentProvider) => BindInstance(argumentProvider);

    protected abstract IModelValue<T> BindInstance(IMappedArgumentProvider argumentProvider);
}