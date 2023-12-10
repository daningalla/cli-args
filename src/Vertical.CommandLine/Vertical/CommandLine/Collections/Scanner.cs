namespace Vertical.CommandLine.Collections;

internal static class ScannerExtensions
{
    internal static Scanner<T> CreateScanner<T>(this T[] array) where T : notnull => new(array);
}

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

    internal bool TryScanValue(T value)
    {
        var matched = Ready && Comparer.Equals(value, Current);
        if (matched) Position++;
        return matched;
    }

    internal bool Advance(int count = 1)
    {
        Position += count;
        return Ready;
    }

    internal ReadOnlySpan<T> UnreadSpan => _array.AsSpan()[Position..];

    internal ReadOnlySpan<T> ReadSpan => _array.AsSpan()[..Position];

    internal ReadOnlySpan<T> Span => _array.AsSpan();

    internal int Position { get; private set; } = 0;

    internal bool Ready => Position < _array.Length;

    internal bool PastEnd => !Ready;

    private T Current => _array[Position];
}