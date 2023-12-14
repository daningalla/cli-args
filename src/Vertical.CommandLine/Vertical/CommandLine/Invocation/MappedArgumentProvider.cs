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
    public T GetValue<T>(string parameterId)
    {
        var binding = (IArgumentValueBinding<T>)Bindings[parameterId];
        return binding.ArgumentValues.Single();
    }

    /// <inheritdoc />
    public T[] GetValueArray<T>(string parameterId) => GetValues<T>(parameterId);

    /// <inheritdoc />
    public List<T> GetValueList<T>(string parameterId) => GetValues<T>(parameterId).ToList();

    /// <inheritdoc />
    public LinkedList<T> GetValueLinkedList<T>(string parameterId) => new(GetValues<T>(parameterId));

    /// <inheritdoc />
    public HashSet<T> GetValueHashSet<T>(string parameterId) => new(GetValues<T>(parameterId));

    /// <inheritdoc />
    public SortedSet<T> GetValueSortedSet<T>(string parameterId) => new(GetValues<T>(parameterId));

    /// <inheritdoc />
    public Stack<T> GetValueStack<T>(string parameterId) => new(GetValues<T>(parameterId));

    /// <inheritdoc />
    public Queue<T> GetValueQueue<T>(string parameterId) => new(GetValues<T>(parameterId));
    
    private T[] GetValues<T>(string parameterId)
    {
        var binding = (ArgumentValueBinding<T>)Bindings[parameterId];

        return binding.ArgumentValues;
    }
}