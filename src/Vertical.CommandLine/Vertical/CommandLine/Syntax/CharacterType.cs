namespace Vertical.CommandLine.Syntax;

/// <summary>
/// Describes a character token's type.
/// </summary>
public enum CharacterType
{
    /// <summary>
    /// Indicates a prefix token, e.g. <c>-</c>, <c>--</c>, or <c>/</c> 
    /// </summary>
    PrefixToken,
    
    /// <summary>
    /// Indicates a character that is part of an identifier name.
    /// </summary>
    IdentifierToken,
    
    /// <summary>
    /// Indicates an invalid character in an identifier name.
    /// </summary>
    InvalidIdentifierToken,
    
    /// <summary>
    /// Indicates an operand assignment character.
    /// </summary>
    OperandAssignmentToken,
    
    /// <summary>
    /// Indicates a character that is part of a literal value.
    /// </summary>
    LiteralValueToken,
    
    /// <summary>
    /// Indicates one of the terminating characters.
    /// </summary>
    TerminatingToken
}