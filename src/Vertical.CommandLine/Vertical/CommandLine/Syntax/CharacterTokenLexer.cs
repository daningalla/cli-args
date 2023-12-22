using Vertical.CommandLine.Collections;

namespace Vertical.CommandLine.Syntax;

/// <summary>
/// Creates sequences of <see cref="CharacterToken"/>.
/// </summary>
public static class CharacterTokenLexer
{
    /// <summary>
    /// Creates a <see cref="CharacterToken"/> sequence from the specified program argument.
    /// </summary>
    /// <param name="arg">Program argument.</param>
    /// <returns>Token list.</returns>
    public static CharacterToken[] GetTokens(string arg)
    {
        if (arg.Length == 0)
        {
            return Array.Empty<CharacterToken>();
        }

        return arg[0] is SyntaxConstants.Hyphen or SyntaxConstants.ForwardSlash
            ? GetPrefixTokens(arg)
            : GetLiteralTokens(arg);
    }

    private static CharacterToken[] GetPrefixTokens(string arg)
    {
        var tokenWriter = new ArrayWriter<CharacterToken>(arg.Length);
        var scanner = new Scanner<char>(arg.ToCharArray());
        var prefixChar = scanner.Scan();

        if (scanner.PastEnd)
        {
            // Single character is always literal
            tokenWriter.Add(new CharacterToken(arg, 0, CharacterType.LiteralValueToken));
            return tokenWriter.ToArray();
        }

        var invalidIdentifier = false;
        var nextChar = scanner.Scan();
        
        switch (nextChar)
        {
            case SyntaxConstants.Hyphen when prefixChar == SyntaxConstants.Hyphen && scanner.PastEnd:
                tokenWriter.Add(new CharacterToken(arg, 0, CharacterType.TerminatingToken));
                tokenWriter.Add(new CharacterToken(arg, 1, CharacterType.TerminatingToken));
                return tokenWriter.ToArray();
            
            case SyntaxConstants.Hyphen:
                tokenWriter.Add(new CharacterToken(arg, 0, CharacterType.PrefixToken));
                tokenWriter.Add(new CharacterToken(arg, 1, CharacterType.PrefixToken));
                break;
            
            default:
                tokenWriter.Add(new CharacterToken(arg, 0, CharacterType.PrefixToken));
                tokenWriter.Add(new CharacterToken(arg, 1, GetFirstIdentifierCharType(nextChar, out invalidIdentifier)));
                break;
        }

        while (scanner.TryScan(chr => chr is not (SyntaxConstants.EqualAssignment or SyntaxConstants.ColonAssignment),
                   out var c))
        {
            tokenWriter.Add(new CharacterToken(
                arg, 
                tokenWriter.WrittenCount, 
                GetNextIdentifierCharType(c, ref invalidIdentifier)));
        }

        if (!scanner.TryScan(c => c is SyntaxConstants.EqualAssignment or SyntaxConstants.ColonAssignment))
            return tokenWriter.ToArray();
        
        tokenWriter.Add(new CharacterToken(arg, tokenWriter.WrittenCount, CharacterType.OperandAssignmentToken));
        
        while (scanner.TryScan(out _))
        {
            tokenWriter.Add(new CharacterToken(
                arg, 
                tokenWriter.WrittenCount, 
                CharacterType.LiteralValueToken));
        }

        return tokenWriter.ToArray();
    }

    private static CharacterType GetFirstIdentifierCharType(char c, out bool invalidIdentifier)
    {
        var result = char.IsLetterOrDigit(c) ? CharacterType.IdentifierToken : CharacterType.InvalidIdentifierToken;
        invalidIdentifier = result == CharacterType.InvalidIdentifierToken;
        return result;
    }

    private static CharacterType GetNextIdentifierCharType(char c, ref bool invalidIdentifier)
    {
        if (invalidIdentifier) return CharacterType.InvalidIdentifierToken;
        
        var result = char.IsLetterOrDigit(c) || c == SyntaxConstants.Hyphen
            ? CharacterType.IdentifierToken
            : CharacterType.InvalidIdentifierToken;
        
        invalidIdentifier = result == CharacterType.InvalidIdentifierToken;
        
        return result;
    }

    private static CharacterToken[] GetLiteralTokens(string arg)
    {
        return Enumerable
            .Range(0, arg.Length)
            .Select(i => new CharacterToken(arg, i, CharacterType.LiteralValueToken))
            .ToArray();
    }
}