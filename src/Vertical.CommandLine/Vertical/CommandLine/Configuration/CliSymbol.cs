namespace Vertical.CommandLine.Configuration;

/// <summary>
/// Represents a CLI symbol that has naming properties.
/// </summary>
public abstract class CliSymbol
{
    /// <summary>
    /// Creates a new instance of this type.
    /// </summary>
    /// <param name="id">The unique identifier of the symbol.</param>
    /// <param name="aliases">Optional aliases the symbol is also known as.</param>
    private protected CliSymbol(string id, string[]? aliases)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        
        Id = id;
        Aliases = aliases ?? Array.Empty<string>();
        Identifiers = new[] { Id }.Concat(Aliases).ToArray();
    }
    
    /// <summary>
    /// Gets the primary symbol mapping id.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the symbol optional aliases.
    /// </summary>
    public string[] Aliases { get; }
    
    /// <summary>
    /// Gets an array that consists of the symbol identifier and any aliases.
    /// </summary>
    public string[] Identifiers { get; }
    
    /// <summary>
    /// Gets the symbol type.
    /// </summary>
    public abstract CliSymbolType SymbolType { get; }
}