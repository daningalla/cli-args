namespace Vertical.CommandLine.Binding.Pipeline;

public class AddBindingSymbolsTask : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        InvokeCore(context);
        next(context);
    }

    private static void InvokeCore(IBindingContext context)
    {
        foreach (var command in context.InvocationPath.Where(cmd => cmd != context.InvocationSubject))
        {
            context.StageBindingSymbols(command
                .Bindings
                .Where(binding => binding.Scope == BindingScope.Descendents));
        }
        
        context.StageBindingSymbols(context
            .InvocationSubject
            .Bindings
            .Where(binding => binding.Scope is BindingScope.Self or BindingScope.SelfAndDescendents));
    }
}