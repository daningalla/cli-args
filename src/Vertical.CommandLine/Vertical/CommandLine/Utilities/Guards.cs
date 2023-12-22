using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Vertical.CommandLine.Utilities;

[ExcludeFromCodeCoverage]
internal static class Guards
{
    internal static void ThrowIfZeroCount(int argument, string? paramName = null)
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