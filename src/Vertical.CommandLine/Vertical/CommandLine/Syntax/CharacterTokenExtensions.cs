namespace Vertical.CommandLine.Syntax;

internal static class CharacterTokenExtensions
{
    internal static string GetString(this ReadOnlySpan<CharacterToken> tokens)
    {
        var chars = new char[tokens.Length];
        for (var c = 0; c < tokens.Length; c++)
        {
            chars[c] = tokens[c].Value;
        }

        return new string(chars);
    }

    internal static string GetString(this CharacterToken[] tokens) => GetString(tokens.AsSpan());
}