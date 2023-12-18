using Vertical.CommandLine.Binding;

namespace Vertical.CommandLine.Invocation;

internal sealed class MappedArgumentProvider : IMappedArgumentProvider
{
    internal MappedArgumentProvider(
        BindingServiceCollection services,
        IEnumerable<IArgumentValueBinding> bindings)
    {
        Services = services;
        Bindings = BindingDictionary<IArgumentValueBinding>.Create(bindings, item => item.BaseSymbol);
    }

    /// <summary>
    /// Gets the binding dictionary keyed by parameter name.
    /// </summary>
    public BindingDictionary<IArgumentValueBinding> Bindings { get; }

    /// <summary>
    /// Gets binding services.
    /// </summary>
    public BindingServiceCollection Services { get; }

    /// <inheritdoc />
    public T GetValue<T>(string bindingId)
    {
        if (Services.TryGetService(out IModelValue<T>? modelValueWrapper))
            return modelValueWrapper.Value;
        
        var binding = (IArgumentValueBinding<T>)Bindings[bindingId];
        return binding.ArgumentValues.Single();
    }

    /// <inheritdoc />
    public T[] GetValueArray<T>(string bindingId) => GetValues<T>(bindingId);

    /// <inheritdoc />
    public List<T> GetValueList<T>(string bindingId) => GetValues<T>(bindingId).ToList();

    /// <inheritdoc />
    public LinkedList<T> GetValueLinkedList<T>(string bindingId) => new(GetValues<T>(bindingId));

    /// <inheritdoc />
    public HashSet<T> GetValueHashSet<T>(string bindingId) => new(GetValues<T>(bindingId));

    /// <inheritdoc />
    public SortedSet<T> GetValueSortedSet<T>(string bindingId) => new(GetValues<T>(bindingId));

    /// <inheritdoc />
    public Stack<T> GetValueStack<T>(string bindingId) => new(GetValues<T>(bindingId));

    /// <inheritdoc />
    public Queue<T> GetValueQueue<T>(string bindingId) => new(GetValues<T>(bindingId));
    
    private T[] GetValues<T>(string parameterId)
    {
        var binding = (ArgumentValueBinding<T>)Bindings[parameterId];
        return binding.ArgumentValues;
    }
}