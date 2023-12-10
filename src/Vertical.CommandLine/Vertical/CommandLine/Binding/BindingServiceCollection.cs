namespace Vertical.CommandLine.Binding;

public sealed class BindingServiceCollection
{
    private readonly Dictionary<Type, IBindingService> _services = new();

    public void Add(IBindingService service)
    {
        ArgumentNullException.ThrowIfNull(service);
        
        _services[service.ServiceType] = service;
    }

    public void AddRange(IEnumerable<IBindingService> services)
    {
        ArgumentNullException.ThrowIfNull(services);

        foreach (var service in services)
        {
            Add(service);
        }
    }

    public T? GetService<T>() where T : IBindingService
    {
        return _services.TryGetValue(typeof(T), out var obj) ? (T)obj : default;
    }

    public T GetRequiredService<T>() where T : IBindingService
    {
        return (T)_services[typeof(T)];
    }

    public void TryAdd(IBindingService? service)
    {
        if (service == null)
            return;

        Add(service);
    }
}