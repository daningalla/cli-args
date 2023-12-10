using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Syntax;

namespace Vertical.CommandLine.Binding;

public sealed class BindingContext : IBindingContext
{
    private readonly List<Command> _invocationPath = new(4);
    private readonly List<IArgumentValueBinding> _argumentValueBindings = new(32);
    private readonly List<SemanticArgument> _semanticArguments = new(32);
    private readonly Dictionary<string, CliBindingSymbol> _bindingSymbols = new(32);
    private readonly Lazy<CliBindingSymbolsCollection> _lazyBindingSymbolCollection;
    private readonly Lazy<SemanticArgumentCollection> _lazySemanticArgumentCollection;
    
    internal BindingContext(RootCommand rootCommand, IEnumerable<string> args, CancellationToken cancellationToken)
    {
        RootCommand = rootCommand;
        ArgumentValues = args.ToArray();
        CancellationToken = cancellationToken;
        
        _lazyBindingSymbolCollection = new Lazy<CliBindingSymbolsCollection>(() =>
            new CliBindingSymbolsCollection(_bindingSymbols.Values));
        _lazySemanticArgumentCollection = new Lazy<SemanticArgumentCollection>(() =>
            new SemanticArgumentCollection(_semanticArguments));
    }

    public RootCommand RootCommand { get; }

    public string[] ArgumentValues { get; }

    /// <inheritdoc />
    public CancellationToken CancellationToken { get; }

    /// <inheritdoc />
    public IEnumerable<Command> InvocationPath => _invocationPath;

    /// <inheritdoc />
    public BindingServiceCollection Services { get; } = new();

    /// <inheritdoc />
    public IEnumerable<CliBindingSymbol> StagedBindingSymbols => _bindingSymbols.Values;

    /// <inheritdoc />
    public IEnumerable<SemanticArgument> StagedSemanticArguments => _semanticArguments;

    /// <inheritdoc />
    public void AddInvocation(Command command) => _invocationPath.Add(command);

    public void AddInvocationArgumentValues(IEnumerable<string> arguments) => InvocationArgumentValues = arguments.ToArray();

    /// <inheritdoc />
    public void AddBindingContext(IArgumentValueBinding binding) => _argumentValueBindings.Add(binding);

    /// <inheritdoc />
    public void StageBindingSymbols(IEnumerable<CliBindingSymbol> symbols)
    {
        if (_lazyBindingSymbolCollection.IsValueCreated)
            throw new InvalidOperationException();
        
        foreach (var symbol in symbols)
        {
            _bindingSymbols[symbol.Id] = symbol;
        }
    }

    /// <inheritdoc />
    public void StageSemanticArguments(SemanticArgument[] arguments)
    {
        if (_lazySemanticArgumentCollection.IsValueCreated)
            throw new InvalidOperationException();
        
        _semanticArguments.AddRange(arguments);
    }

    /// <inheritdoc />
    public CliBindingSymbolsCollection GetBindingSymbols() => _lazyBindingSymbolCollection.Value;

    /// <inheritdoc />
    public SemanticArgumentCollection GetSemanticArguments() => _lazySemanticArgumentCollection.Value;

    /// <inheritdoc />
    public IEnumerable<IArgumentValueBinding> GetValueBindings() => _argumentValueBindings;

    /// <inheritdoc />
    public Command InvocationSubject => _invocationPath.Last() ?? throw new InvalidOperationException();

    public IEnumerable<string> InvocationArgumentValues { get; private set; } = Enumerable.Empty<string>();
}