using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Syntax;

/// <summary>
/// Represents a command line argument that has semantic meaning.
/// </summary>
public class SemanticArgument
{
    private readonly CharacterToken[] _tokens;
    private readonly SemanticAnatomy _anatomy;

    internal SemanticArgument(
        int ordinal,
        TokenizedInputSequence sequence,
        SemanticHint semanticHint = SemanticHint.None)
        : this(ordinal, sequence, SemanticAnatomy.Empty, semanticHint)
    {
    }
    
    internal SemanticArgument(
        int ordinal,
        TokenizedInputSequence sequence,
        SemanticAnatomy anatomy,
        SemanticHint semanticHint = SemanticHint.None)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        Guards.ThrowIfZeroCount(sequence.Length, nameof(sequence));

        _anatomy = anatomy;
        _tokens = sequence.TokenArray;
        Ordinal = ordinal;
        SemanticHint = semanticHint;
        Text = _tokens.GetString();
    }

    /// <summary>
    /// Gets the argument's ordinal position.
    /// </summary>
    public int Ordinal { get; }

    /// <summary>
    /// Gets the character tokens for the argument.
    /// </summary>
    public ReadOnlySpan<CharacterToken> Tokens => _tokens.AsSpan();

    /// <summary>
    /// Gets the prefixed input sequence.
    /// </summary>
    public ref readonly SemanticAnatomy Anatomy => ref _anatomy;

    /// <summary>
    /// Gets the semantic hint for this argument.
    /// </summary>
    public SemanticHint SemanticHint { get; }

    /// <summary>
    /// Gets the full text of the argument.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Gets whether this instance represents an option or switch.
    /// </summary>
    public bool IsOption => _anatomy.PrefixFormat != IdentifierFormat.None;

    /// <summary>
    /// Gets whether this instance represents a known switch.
    /// </summary>
    public bool IsKnownSwitch => SemanticHint == SemanticHint.KnownSwitch;

    /// <summary>
    /// Gets whether this instance contains an attached operand value.
    /// </summary>
    public bool HasOperand => _anatomy.OperandValueSpan.Length > 0;

    /// <summary>
    /// Gets whether this instance represents an argument.
    /// </summary>
    public bool IsDiscreetArgument => _anatomy.PrefixFormat == IdentifierFormat.None;

    /// <inheritdoc />
    public override string ToString() => $"\"{Text}\"";
}