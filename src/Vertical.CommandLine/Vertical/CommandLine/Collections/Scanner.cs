namespace Vertical.CommandLine.Collections;

internal ref struct Scanner<T> where T : notnull
{
    private static readonly EqualityComparer<T> Comparer = EqualityComparer<T>.Default;
    private readonly T[] _array;

    internal Scanner(T[] array)
    {
        _array = array;
    }

    internal T Scan()
    {
        return TryScan(out var value)
            ? value
            : throw new InvalidOperationException();
    }

    internal bool TryScan(out T value)
    {
        var ready = Ready;
        value = ready ? Current : default!;
        Position++;
        return ready;
    }

    internal bool TryScan(Predicate<T> predicate) => TryScan(predicate, out _);

    internal bool TryScan(Predicate<T> predicate, out T value)
    {
        var ready = Ready;
        value = ready ? Current : default!;
        var matched = ready && predicate(value);
        if (matched) Position++;
        return matched;
    }

    internal int Position { get; private set; } = 0;

    internal bool Ready => Position < _array.Length;

    internal bool PastEnd => !Ready;

    private T Current => _array[Position];
}