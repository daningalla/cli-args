namespace Vertical.CommandLine;

/// <summary>
/// Defines the binding scopes for arguments, options, and switches.
/// </summary>
public enum BindingScope
{
    /// <summary>
    /// Indicates the argument, option, or switch only applies to the command in which it is defined.
    /// </summary>
    Self,
    
    /// <summary>
    /// Indicates the argument, option, or switch applies to the command or sub-commands in which it is
    /// defined.
    /// </summary>
    SelfAndDescendents,
    
    /// <summary>
    /// Indicates the argument, option, or switch applies to sub-commands of the command in which it is defined.
    /// </summary>
    Descendents
}