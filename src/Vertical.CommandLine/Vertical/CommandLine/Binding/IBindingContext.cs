using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Invocation;
using Vertical.CommandLine.Syntax;

namespace Vertical.CommandLine.Binding;

/// <summary>
/// Represents an objects that maintains binding state.
/// </summary>
public interface IBindingContext
{
    /// <summary>
    /// Gets the root command.
    /// </summary>
    RootCommand RootCommand { get; }
    
    /// <summary>
    /// Gets the original supplied argument values.
    /// </summary>
    string[] ArgumentValues { get; }
    
    /// <summary>
    /// Gets the cancellation token.
    /// </summary>
    CancellationToken CancellationToken { get; }
    
    /// <summary>
    /// Gets the command invocation path. The commands are ordered beginning with the
    /// root command and the sub command instances that are contained in the path.
    /// </summary>
    IEnumerable<Command> InvocationPath { get; }

    /// <summary>
    /// Gets the command being invoked (equivalent to <c>InvocationPath.Last()</c>.
    /// </summary>
    Command InvocationSubject { get; }

    /// <summary>
    /// Gets binding services such as command defined validators and converters.
    /// </summary>
    BindingServiceCollection Services { get; }
    
    /// <summary>
    /// Gets the arguments that are handled by the invocation subject.
    /// </summary>
    IEnumerable<string> InvocationArgumentValues { get; }
    
    /// <summary>
    /// Gets binding symbols that have been staged for mapping.
    /// </summary>
    IEnumerable<CliBindingSymbol> StagedBindingSymbols { get; }
    
    /// <summary>
    /// Gets the semantic arguments that have been staged for mapping.
    /// </summary>
    IEnumerable<SemanticArgument> StagedSemanticArguments { get; }
    
    /// <summary>
    /// Adds an invocation to the path.
    /// </summary>
    /// <param name="command">The command.</param>
    void AddInvocation(Command command);

    /// <summary>
    /// Adds invocation argument values.
    /// </summary>
    /// <param name="arguments">The arguments to stage.</param>
    void AddInvocationArgumentValues(IEnumerable<string> arguments);

    /// <summary>
    /// Adds a binding context.
    /// </summary>
    /// <param name="symbol">The symbol being bound.</param>
    /// <param name="values">Zero, one, or more values to bind.</param>
    void AddBindingContext(CliBindingSymbol symbol, IEnumerable<string> values);

    /// <summary>
    /// Adds a bound model.
    /// </summary>
    /// <param name="modelValue">The bound model value.</param>
    void AddModelValue(IModelValue modelValue);
    
    /// <summary>
    /// Stages the specified binding symbols.
    /// </summary>
    /// <param name="symbols">Symbols that will be bound later.</param>
    void StageBindingSymbols(IEnumerable<CliBindingSymbol> symbols);

    /// <summary>
    /// Stages the specified semantic argument symbols.
    /// </summary>
    /// <param name="arguments">Arguments that will be bound later.</param>
    void StageSemanticArguments(SemanticArgument[] arguments);

    /// <summary>
    /// Gets an index of the finalized collection of binding symbols.
    /// </summary>
    /// <returns><see cref="CliBindingSymbolsCollection"/></returns>
    CliBindingSymbolsCollection GetBindingSymbols();

    /// <summary>
    /// Gets an index of the finalized semantic argument.
    /// </summary>
    /// <returns><see cref="SemanticArgumentCollection"/></returns>
    SemanticArgumentCollection GetSemanticArguments();

    /// <summary>
    /// Gets the argument bindings that will be used to invoke the command handler.
    /// </summary>
    /// <returns>Collection of value bindings.</returns>
    IEnumerable<IArgumentValueBinding> GetValueBindings();

    /// <summary>
    /// Creates an argument provider using the current state of the context.
    /// </summary>
    /// <returns><see cref="IMappedArgumentProvider"/></returns>
    IMappedArgumentProvider CreateArgumentProvider();
}