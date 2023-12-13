using Vertical.CommandLine.Binding;
using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine;

/// <summary>
/// Represents a command, which identifies sub-functionality of an application.
/// </summary>
public class Command : CliSymbol
{
    /// <summary>
    /// Initializes a new instance of a <see cref="Command"/>.
    /// </summary>
    /// <param name="id">The unique id of the command.</param>
    /// <param name="aliases">Other names the command can be referred to by.</param>
    public Command(string id, string[]? aliases = null) : base(id, aliases)
    {
    }
    
    private protected Command(string id) : base(id, aliases: null)
    {
    }
    
    /// <summary>
    /// Gets the delegate that implements the logic when this command is invoked.
    /// </summary>
    public Delegate? Handler { get; init; }
    
    /// <summary>
    /// Gets the sub-commands for this instance.
    /// </summary>
    public List<Command> Commands { get; } = new();

    /// <summary>
    /// Gets the options, arguments, and switches defined for the command.
    /// </summary>
    public List<CliBindingSymbol> Bindings { get; } = new();

    /// <summary>
    /// Gets a list of converters.
    /// </summary>
    public List<IValueConverter> Converters { get; } = new();

    /// <summary>
    /// Gets a list of validators.
    /// </summary>
    public List<IValidator> Validators { get; } = new();

    /// <inheritdoc />
    public override string ToString() => '"' + string.Join(" | ", new[] { Id }.Concat(Aliases)) + '"';

    /// <inheritdoc />
    public override CliSymbolType SymbolType => CliSymbolType.Command;
}