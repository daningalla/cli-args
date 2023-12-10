namespace Vertical.CommandLine.Utilities;

internal static class MemoryUtilities
{
    internal static Range ScanWhile<T>(
        this ReadOnlySpan<T> span,
        Predicate<T> predicate,
        ref int ptr)
    {
        var start = ptr;

        for (; ptr < span.Length && predicate(span[ptr]); ptr++)
        {
        }

        return new Range(start, ptr);
    }

    internal static void Scan<T>(
        this ReadOnlySpan<T> span,
        Action<T> action)
    {
        for (var c = 0; c < span.Length; c++)
        {
            action(span[c]);
        }
    }
    
    internal static TState Scan<TState, T>(
        this ReadOnlySpan<T> span,
        TState state,
        Func<TState, T, TState> scanner)
    {
        for (var c = 0; c < span.Length; c++)
        {
            state = scanner(state, span[c]);
        }

        return state;
    }
}