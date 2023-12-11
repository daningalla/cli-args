using System.Diagnostics.CodeAnalysis;

namespace Vertical.CommandLine.Syntax;

/// <summary>
/// Represents a tokenized form of an input argument.
/// </summary>
public readonly struct TokenizedInputSequence : IEquatable<TokenizedInputSequence>
{
    /// <summary>
    /// Initializes a new <see cref="TokenizedInputSequence"/> structure.
    /// </summary>
    /// <param name="tokenArray">The tokens within the sequence.</param>
    public TokenizedInputSequence(CharacterToken[] tokenArray)
    {
        ArgumentNullException.ThrowIfNull(tokenArray);
        
        TokenArray = tokenArray;
        Text = new string(tokenArray.Select(token => token.Value).ToArray());
    }

    /// <summary>
    /// Gets an empty instance.
    /// </summary>
    public static TokenizedInputSequence Empty => new(Array.Empty<CharacterToken>());

    /// <summary>
    /// Gets the tokens in the sequence.
    /// </summary>
    public CharacterToken[] TokenArray { get; }

    /// <summary>
    /// Gets the full text of the sequence.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Gets the token span.
    /// </summary>
    public ReadOnlySpan<CharacterToken> Span => TokenArray.AsSpan();

    /// <summary>
    /// Gets the length of the sequence.
    /// </summary>
    public int Length => TokenArray.Length;

    /// <summary>
    /// Gets the token at the specified index.
    /// </summary>
    /// <param name="index">Index.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public CharacterToken this[int index] => TokenArray[index];

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => Text;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => Text.GetHashCode();

    /// <inheritdoc />
    public bool Equals(TokenizedInputSequence other) => Text == other.Text;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is TokenizedInputSequence other && Equals(other);

    public static bool operator==(TokenizedInputSequence x, TokenizedInputSequence y) => x.Equals(y);
    public static bool operator!=(TokenizedInputSequence x, TokenizedInputSequence y) => !x.Equals(y);
}