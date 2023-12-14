namespace Vertical.CommandLine.Binding.Pipeline;

public sealed class AddValueConverterServicesTask : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        InvokeCore(context);
        next(context);
    }

    private static void InvokeCore(IBindingContext context)
    {
        var converterTypes = context
            .InvocationPath
            .SelectMany(command => command.Converters)
            .Aggregate(new HashSet<Type>(), (state, next) =>
            {
                context.Services.Add(next);
                state.Add(next.ValueType);
                return state;
            });

        var bindings = context
            .InvocationPath
            .SelectMany(command => command.Bindings)
            .Where(binding => !binding.HasConverter && !converterTypes.Contains(binding.ValueType));
        
        context.Services.AddRange(bindings.Select(binding => binding.GetDefaultConverter()
            ?? throw new InvalidOperationException()));
    }
}