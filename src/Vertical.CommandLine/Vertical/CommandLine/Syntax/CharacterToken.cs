namespace Vertical.CommandLine.Syntax;

/// <summary>
/// Represents a character token used in parsing.
/// </summary>
public readonly struct CharacterToken : IEquatable<CharacterToken>
{
    private readonly string _text;

    internal CharacterToken(
        string text,
        int index,
        CharacterType type)
    {
        _text = text;
        Index = index;
        Type = type;
    }

    /// <summary>
    /// Gets the index of the token within the argument.
    /// </summary>
    public int Index { get; }
    
    /// <summary>
    /// Gets the character type.
    /// </summary>
    public CharacterType Type { get; }
    
    /// <summary>
    /// Gets the full span of the text in which the token is located.
    /// </summary>
    public ReadOnlySpan<char> FullSpan => _text.AsSpan();

    /// <summary>
    /// Gets the token value.
    /// </summary>
    public char Value => _text[Index];

    /// <inheritdoc />
    public override string ToString() => $"{Type} '{Value}'";

    /// <inheritdoc />
    public bool Equals(CharacterToken other) => _text == other._text && Index == other.Index && Type == other.Type;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is CharacterToken other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(_text, Index, (int)Type);

    public static bool operator ==(CharacterToken x, CharacterToken y) => x.Equals(y);
    public static bool operator !=(CharacterToken x, CharacterToken y) => !x.Equals(y);
}