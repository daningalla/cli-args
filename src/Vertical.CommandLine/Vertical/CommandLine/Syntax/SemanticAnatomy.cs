using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Syntax;

public sealed class SemanticAnatomy
{
    private readonly TokenizedInputSequence _tokens;
    private readonly Range _prefixRange;
    private readonly Range _identifierRange;
    private readonly Range _assignmentOperatorRange;
    private readonly Range _operandValueRange;

    private SemanticAnatomy(
        TokenizedInputSequence tokens,
        Range prefixRange,
        Range identifierRange,
        Range assignmentOperatorRange,
        Range operandValueRange)
    {
        _tokens = tokens;
        _prefixRange = prefixRange;
        _identifierRange = identifierRange;
        _assignmentOperatorRange = assignmentOperatorRange;
        _operandValueRange = operandValueRange;
        
        PrefixFormat = _tokens.Span[prefixRange] switch
        {
            [{ Value: SyntaxConstants.Hyphen }] => IdentifierFormat.Posix,
            [{ Value: SyntaxConstants.Hyphen }, { Value: SyntaxConstants.Hyphen}] => IdentifierFormat.Gnu,
            [{ Value: SyntaxConstants.ForwardSlash }] => IdentifierFormat.Microsoft,
            _ => IdentifierFormat.None
        };
    }

    /// <summary>
    /// Gets an empty instance of this type.
    /// </summary>
    public static SemanticAnatomy Empty { get; } = new(TokenizedInputSequence.Empty,
        new Range(0, 0),
        new Range(0, 0),
        new Range(0, 0),
        new Range(0, 0));

    private ReadOnlySpan<CharacterToken> FullSpan => _tokens.Span;

    /// <summary>
    /// Gets the prefix.
    /// </summary>
    public string Prefix => FullSpan[_prefixRange].GetString();

    /// <summary>
    /// Gets the identifier span.
    /// </summary>
    public ReadOnlySpan<CharacterToken> IdentifierSpan => FullSpan[_identifierRange];

    /// <summary>
    /// Gets the identifier without the prefix.
    /// </summary>
    public string Identifier => IdentifierSpan.GetString();

    /// <summary>
    /// Gets the identifier name with the prefix.
    /// </summary>
    public string PrefixedIdentifier => FullSpan[_prefixRange.Start.._identifierRange.End].GetString();

    /// <summary>
    /// Gets the operand assignment operator;
    /// </summary>
    public string OperandAssignmentOperator => FullSpan[_assignmentOperatorRange].GetString();

    /// <summary>
    /// Gets the span of the operand value.
    /// </summary>
    public ReadOnlySpan<CharacterToken> OperandValueSpan => FullSpan[_operandValueRange];

    /// <summary>
    /// Gets the operand value.
    /// </summary>
    public string OperandValue => FullSpan[_operandValueRange].GetString();

    /// <summary>
    /// Gets the operand expression.
    /// </summary>
    public string OperandExpression => FullSpan[_assignmentOperatorRange.Start..].GetString();

    /// <summary>
    /// Gets the prefix format.
    /// </summary>
    public IdentifierFormat PrefixFormat { get; }

    internal static SemanticAnatomy Create(TokenizedInputSequence inputSequence)
    {
        var span = inputSequence.Span;
        var index = 0; 

        return new SemanticAnatomy(
            inputSequence,
            span.ScanWhile(token => token.Type == CharacterType.PrefixToken, ref index),
            span.ScanWhile(token => token.Type != CharacterType.OperandAssignmentToken, ref index),
            span.ScanWhile(token => token.Type == CharacterType.OperandAssignmentToken, ref index),
            span.ScanWhile(_ => true, ref index));
    }

    /// <inheritdoc />
    public override string ToString() => _tokens.ToString();
}