using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Vertical.CommandLine.Utilities;

[ExcludeFromCodeCoverage]
internal static class Guards
{
    internal static void ThrowIfEmpty<T>(
        IEnumerable<T> argument,
        [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument.Any())
            return;
        
        ThrowEmpty(paramName);
    }
    
    internal static void ThrowIfZeroCount(int argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument > 0)
            return;
        
        ThrowEmpty(paramName);
    }

    [DoesNotReturn]
    private static void ThrowEmpty(string? paramName)
    {
        throw new ArgumentException("Sequence cannot be empty.", paramName);
    }
}