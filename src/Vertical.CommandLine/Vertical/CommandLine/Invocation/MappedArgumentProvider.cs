using Vertical.CommandLine.Binding;

namespace Vertical.CommandLine.Invocation;

internal sealed class MappedArgumentProvider : IMappedArgumentProvider
{
    private readonly IReadOnlyDictionary<string, IArgumentValueBinding> _bindings;
    
    internal MappedArgumentProvider(
        BindingServiceCollection services,
        IEnumerable<IArgumentValueBinding> bindings)
    {
        Services = services;
        _bindings = bindings.ToDictionary(
            binding => binding.BindingId,
            binding => binding,
            BindingNameComparer.Instance);
    }

    /// <summary>
    /// Gets binding services.
    /// </summary>
    public BindingServiceCollection Services { get; }

    /// <inheritdoc />
    public T GetValue<T>(string bindingId)
    {
        if (Services.TryGetService(out IModelValue<T>? modelValueWrapper))
            return modelValueWrapper.Value;

        var binding = GetBinding<T>(bindingId);
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
        var binding = GetBinding<T>(parameterId);
        return binding.ArgumentValues;
    }

    private ArgumentValueBinding<T> GetBinding<T>(string id)
    {
        return (ArgumentValueBinding<T>)_bindings[id];
    }
}