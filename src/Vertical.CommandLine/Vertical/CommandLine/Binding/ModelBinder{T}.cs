using Vertical.CommandLine.Invocation;

namespace Vertical.CommandLine.Binding;

public class ModelBinder<T> : IModelBinder
{
    /// <inheritdoc />
    public Type ValueType => typeof(T);

    /// <inheritdoc />
    public Type ServiceType => typeof(IModelBinder);

    /// <inheritdoc />
    public IModelValue BindInstanceBase(IMappedArgumentProvider argumentProvider) =>
        new ModelValue<T>(BindInstance(argumentProvider));

    protected virtual T BindInstance(IMappedArgumentProvider argumentProvider)
    {
        throw new NotImplementedException("Model binding is not implemented.");
    }
}