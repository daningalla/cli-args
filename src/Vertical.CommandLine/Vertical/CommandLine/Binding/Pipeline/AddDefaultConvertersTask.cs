using Vertical.CommandLine.Conversion;

namespace Vertical.CommandLine.Binding.Pipeline;

public sealed class AddDefaultConvertersTask : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        InvokeCore(context);
        next(context);
    }

    private static void InvokeCore(IBindingContext context)
    {
        var types = context
            .InvocationPath
            .SelectMany(command => command
                .Bindings
                .Where(binding => !binding.HasConverter)
                .Select(binding => binding.ValueType))
            .Distinct();
        
        context.Services.AddRange(types.Select(type => DefaultValueConverter.GetInstanceOrDefault(type)
            ?? throw new InvalidOperationException()));
    }
}