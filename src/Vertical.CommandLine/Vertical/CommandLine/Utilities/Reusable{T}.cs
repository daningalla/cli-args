namespace Vertical.CommandLine.Utilities;

internal sealed class Reusable<T> where T : class
{
    internal sealed class LeasedValue : IDisposable
    {
        private readonly Reusable<T>? _owner;

        internal LeasedValue(T value, Reusable<T>? owner)
        {
            _owner = owner;
            Value = value;
        }

        public void Dispose() => _owner?.Return(Value);

        public T Value { get; }
    }
    
    private readonly Func<T> _factory;
    private readonly Action<T>? _resetAction;
    private T? _instance;

    internal Reusable(Func<T> factory, Action<T>? resetAction = null)
    {
        _factory = factory;
        _resetAction = resetAction;
        _instance = factory();
    }

    internal LeasedValue GetInstance()
    {
        var instance = Interlocked.Exchange(ref _instance, null);

        return instance != null
            ? new LeasedValue(instance, this)
            : new LeasedValue(_factory(), null);
    }

    private void Return(T value)
    {
        _resetAction?.Invoke(value);
        Interlocked.Exchange(ref _instance, value);
    }
}