using Vertical.CommandLine.Binding;
using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Invocation;

internal sealed class ValidatingArgumentProvider : IMappedArgumentProvider
{
    private readonly Type _modelType;
    private readonly ICollection<Exception> _exceptions;
    private readonly HashSet<string> _bindingIdHashSet;

    internal ValidatingArgumentProvider(
        BindingServiceCollection services,
        IEnumerable<string> bindingIds,
        Type modelType,
        ICollection<Exception> exceptions)
    {
        _modelType = modelType;
        _exceptions = exceptions;
        Services = services;
        _bindingIdHashSet = new HashSet<string>(bindingIds, new BindingNameComparer());
    }
    
    /// <inheritdoc />
    public T GetValue<T>(string bindingId)
    {
        ValidateKeyRequest<T>(bindingId);
        return default!;
    }

    /// <inheritdoc />
    public T[] GetValueArray<T>(string bindingId)
    {
        ValidateKeyRequest<T>(bindingId);
        return Array.Empty<T>();
    }

    /// <inheritdoc />
    public List<T> GetValueList<T>(string bindingId)
    {
        ValidateKeyRequest<T>(bindingId);
        return new List<T>();
    }

    /// <inheritdoc />
    public LinkedList<T> GetValueLinkedList<T>(string bindingId)
    {
        ValidateKeyRequest<T>(bindingId);
        return new LinkedList<T>();
    }

    /// <inheritdoc />
    public HashSet<T> GetValueHashSet<T>(string bindingId)
    {
        ValidateKeyRequest<T>(bindingId);
        return new HashSet<T>();
    }

    /// <inheritdoc />
    public SortedSet<T> GetValueSortedSet<T>(string bindingId)
    {
        ValidateKeyRequest<T>(bindingId);
        return new SortedSet<T>();
    }

    /// <inheritdoc />
    public Stack<T> GetValueStack<T>(string bindingId)
    {
        ValidateKeyRequest<T>(bindingId);
        return new Stack<T>();
    }

    /// <inheritdoc />
    public Queue<T> GetValueQueue<T>(string bindingId)
    {
        ValidateKeyRequest<T>(bindingId);
        return new Queue<T>();
    }

    /// <inheritdoc />
    public BindingServiceCollection Services { get; }

    private void ValidateKeyRequest<T>(string id)
    {
        if (_bindingIdHashSet.Contains(id))
            return;
        
        _exceptions.Add(ConfigurationExceptions.ModelHasUnbindablePropertyOrConstructorParameter(
            _modelType,
            id,
            typeof(T)));
    }
}