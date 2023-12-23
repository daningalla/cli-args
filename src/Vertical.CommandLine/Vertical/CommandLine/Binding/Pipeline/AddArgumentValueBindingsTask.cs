using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Syntax;

namespace Vertical.CommandLine.Binding.Pipeline;

internal sealed class AddArgumentValueBindingsTask : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        InvokeCore(context);
        next(context);
    }

    private static void InvokeCore(IBindingContext context)
    {
        var symbols = context.GetBindingSymbols();
        var argumentCollection = context.GetSemanticArguments();
        var argumentQueue = new Queue<SemanticArgument>(argumentCollection.Where(arg => !arg.IsOption));

        while (symbols.TryRemoveArgumentBinding(out var symbol))
        {
            var arguments = DequeueArguments(symbol, argumentQueue).ToArray();
            
            symbol.ValidateArity(arguments.Length, () => arguments.Select(arg => arg.Text));

            var argumentValues = arguments
                .Select(arg =>
                {
                    argumentCollection.RemoveArgument(arg);
                    return arg.Text;
                })
                .ToArray();

            context.AddBindingContext(symbol, argumentValues);
        }
    }

    private static IEnumerable<SemanticArgument> DequeueArguments(
        CliBindingSymbol symbol, 
        Queue<SemanticArgument> argumentQueue)
    {
        var (count, maxCount) = (0, symbol.Arity.MaxCount ?? int.MaxValue);

        while (count++ < maxCount && argumentQueue.TryDequeue(out var argument))
        {
            yield return argument;
        }
    }
}