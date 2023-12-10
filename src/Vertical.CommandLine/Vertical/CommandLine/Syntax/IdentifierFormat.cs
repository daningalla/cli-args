namespace Vertical.CommandLine.Syntax;

/// <summary>
/// Defines the prefix styles for an identifier.
/// </summary>
public enum IdentifierFormat
{
    /// <summary>
    /// No prefix style.
    /// </summary>
    None,
    
    /// <summary>
    /// Indicates the POSIX short-form prefix style, e.g. <c>-</c>
    /// </summary>
    Posix,
    
    /// <summary>
    /// Indicates the GNU long-form prefix style, e.g. <c>--</c>.
    /// </summary>
    Gnu,
    
    /// <summary>
    /// Indicates the Microsoft style prefix style, e.g. <c>/</c>.
    /// </summary>
    Microsoft
}