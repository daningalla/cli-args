namespace Vertical.CommandLine.Binding.Pipeline;

public static class BindingPipeline
{
    internal static readonly Action<IBindingContext> TerminalTask = _ => { };
    
    private static readonly Func<IBindingTask[]> DefaultMiddlewareFactory = () => new IBindingTask[]
    {
#if DEBUG        
        new ValidateConfigurationTask(),
#endif
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

    public static LinkedList<IBindingTask> DefaultMiddleware => new(DefaultMiddlewareFactory());

    public static IBindingContext CreateContext(
        RootCommand rootCommand,
        IEnumerable<string> args,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rootCommand);
        ArgumentNullException.ThrowIfNull(args);

        return CreateContext(rootCommand, args, DefaultMiddlewareFactory(), cancellationToken);
    }

    public static IBindingContext CreateContext(
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

    public static IBindingContext Build(
        IEnumerable<IBindingTask> middlewares,
        IBindingContext context)
    {
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