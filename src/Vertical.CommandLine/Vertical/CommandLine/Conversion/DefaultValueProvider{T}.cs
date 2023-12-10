namespace Vertical.CommandLine.Conversion;

internal sealed class DefaultValueProvider<T> : IDefaultValueProvider<T>
{
    private readonly Func<T> _provider;

    internal DefaultValueProvider(Func<T>? provider = null)
    {
        _provider = provider ?? new Func<T>(() => default!);
    }

    internal static IDefaultValueProvider<T> Instance { get; } = new DefaultValueProvider<T>();
    
    /// <inheritdoc />
    public Type ServiceType => typeof(T);

    /// <inheritdoc />
    public T Value => _provider();
}