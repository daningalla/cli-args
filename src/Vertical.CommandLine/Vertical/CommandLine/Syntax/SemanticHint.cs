namespace Vertical.CommandLine.Syntax;

/// <summary>
/// Defines hints for semantic arguments.
/// </summary>
public enum SemanticHint
{
    None,
 
    /// <summary>
    /// Indicates the argument is a known switch.
    /// </summary>
    KnownSwitch,
    
    /// <summary>
    /// Indicates the argument is known argument value.
    /// </summary>
    DiscreetArgument,
    
    /// <summary>
    /// Indicates the argument may be an operand value for an option.
    /// </summary>
    SpeculativeOperand,
    
    /// <summary>
    /// Indicates the arguments comes after a terminating symbol.
    /// </summary>
    Terminated
}