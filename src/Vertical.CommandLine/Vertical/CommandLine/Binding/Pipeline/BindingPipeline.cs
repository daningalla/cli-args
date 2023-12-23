namespace Vertical.CommandLine.Binding.Pipeline;

internal static class BindingPipeline
{
    // ReSharper disable once MemberCanBePrivate.Global
    internal static readonly Action<IBindingContext> TerminalTask = _ => { };
    
    private static readonly Func<IBindingTask[]> DefaultMiddlewareFactory = () => new IBindingTask[]
    {
        new BuildCommandPathTask(),
        new AddBindingSymbolsTask(),
        new AddValueConverterServicesTask(),
        new AddValidatorServicesTask(),
        new PrepareSemanticArgumentsTask(),
        new AddOptionValueBindingsTask(),
        new AddArgumentValueBindingsTask(),
        new AddModelBindingValuesTasks(),
        new PostValidateContextTask()
    };

    internal static IBindingContext CreateContext(
        RootCommand rootCommand,
        IEnumerable<string> args,
        CancellationToken cancellationToken)
    {
        return CreateContext(rootCommand, args, DefaultMiddlewareFactory(), cancellationToken);
    }

    private static IBindingContext CreateContext(
        RootCommand rootCommand,
        IEnumerable<string> args,
        IEnumerable<IBindingTask> middlewares,
        CancellationToken cancellationToken)
    {
        var context = new BindingContext(rootCommand, args, cancellationToken);
        var pipeline = BuildPipeline(middlewares);

        pipeline(context);

        return context;
    }

    private static Action<IBindingContext> BuildPipeline(IEnumerable<IBindingTask> middlewares)
    {
        var action = TerminalTask;

        foreach (var middleware in middlewares.Reverse())
        {
            var next = action;
            action = context => middleware.Invoke(context, next);
        }

        return action;
    }
}