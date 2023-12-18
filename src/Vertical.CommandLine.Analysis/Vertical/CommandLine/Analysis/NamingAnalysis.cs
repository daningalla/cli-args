namespace Vertical.CommandLine.Analysis;

public static class NamingAnalysis
{
    public static bool IsValidPosixIdentifier(ReadOnlySpan<char> span)
    {
        if (span.Length < 2) return false;

        if (span[0] != '-') return false;
        
        span = span.Slice(1);

        // Each character must be unique
        if (new HashSet<char>(span.ToArray()).Count != span.Length)
            return false;

        for (var i = 1; i < span.Length; i++)
        {
            var c = span[i];

            if (!char.IsLetterOrDigit(c))
                return false;
        }

        return true;
    }

    public static bool IsValidGnuIdentifier(ReadOnlySpan<char> span)
    {
        if (span.Length < 3) return false;

        if (!(span[0] == '-' && span[1] == '-'))
            return false;

        span = span.Slice(2);

        for (var i = 2; i < span.Length; i++)
        {
            var c = span[i];

            if (char.IsLetterOrDigit(c) || c == '-')
                continue;

            return false;
        }

        return true;
    }

    public static bool IsValidMicrosoftIdentifier(ReadOnlySpan<char> span)
    {
        if (span.Length < 2) return false;
        
        if (span[0] != '/') return false;

        for (var c = 1; c < span.Length; c++)
        {
            if (!char.IsLetterOrDigit(span[c]))
                return false;
        }

        return true;
    }

    public static bool IsValidPrefixedIdentifier(ReadOnlySpan<char> span) =>
        IsValidPosixIdentifier(span) ||
        IsValidGnuIdentifier(span) ||
        IsValidMicrosoftIdentifier(span);

    public static bool IsValidNonPrefixedIdentifier(ReadOnlySpan<char> span)
    {
        if (span.Length == 0) return false;

        if (!char.IsLower(span[0]))
            return false;

        for (var c = 1; c < span.Length; c++)
        {
            if (char.IsLower(span[c]) || span[c] == '-')
                continue;

            return false;
        }

        return true;
    }
}