using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Syntax;

namespace Vertical.CommandLine.Binding.Pipeline;

public class AddOptionValueBindingsTask : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        InvokeCore(context);
        next(context);
    }

    private static void InvokeCore(IBindingContext context)
    {
        var argumentCollection = context.GetSemanticArguments();

        var bindingSymbols = context.GetBindingSymbols();
        
        while (bindingSymbols.TryRemoveOptionBinding(out var symbol))
        {
            var argumentPairs = argumentCollection.RemoveOptionArguments(symbol).ToArray();
            var argumentValues = BuildArgumentValues(
                    argumentCollection, 
                    argumentPairs)
                .ToArray();

            symbol.ValidateArity(argumentValues.Length, () => argumentValues);

            _ = symbol.SymbolType == CliSymbolType.Option
                ? AddOptionBindingContext(context, symbol, argumentValues)
                : AddSwitchBindingContext(context, symbol, argumentValues);
        }
    }

    private static int AddSwitchBindingContext(
        IBindingContext context,
        CliBindingSymbol symbol,
        IEnumerable<string?> argumentValues)
    {
        var value = argumentValues.FirstOrDefault();
        var bindingValues = value != null
            ? new[] { value }
            : Enumerable.Empty<string>();
        
        context.AddBindingContext(symbol, bindingValues);
        return 0;
    }

    private static int AddOptionBindingContext(
        IBindingContext context,
        CliBindingSymbol symbol,
        IReadOnlyCollection<string?> argumentValues)
    {
        if (argumentValues.Any(str => str is null))
        {
            throw CommandLineException.OptionMissingOperand(symbol);
        }

        context.AddBindingContext(symbol, argumentValues.Cast<string>());
        return 0;
    }

    private static IEnumerable<string?> BuildArgumentValues(
        SemanticArgumentCollection argumentCollection,
        IEnumerable<(SemanticArgument MatchedArgument, SemanticArgument? SpeculativeOperand)> pairs)
    {
        return pairs.Select(pair => BuildArgumentValue(argumentCollection, pair));
    }

    private static string? BuildArgumentValue(
        SemanticArgumentCollection argumentCollection,
        (SemanticArgument MatchedArgument, SemanticArgument? SpeculativeOperand) pair)
    {
        switch (pair)
        {
            case { MatchedArgument.HasOperand: true }:
                return pair.MatchedArgument.Anatomy.OperandValue;
            
            case { SpeculativeOperand: not null }:
                argumentCollection.RemoveArgument(pair.SpeculativeOperand);
                return pair.SpeculativeOperand.Text;
            
            default:
                return null;
        }
    }
}