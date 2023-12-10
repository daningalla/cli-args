using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine;

/// <summary>
/// Represents the root command, which identifies the main functionality of the application. 
/// </summary>
public sealed class RootCommand : Command
{
    /// <summary>
    /// Gets the root command id.
    /// </summary>
    public const string RootId = "(root)";
    
    /// <inheritdoc />
    public RootCommand() : base(RootId)
    {
    }

    /// <inheritdoc />
    public override CliSymbolType SymbolType => CliSymbolType.RootCommand;
}