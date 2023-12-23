using Vertical.CommandLine.Invocation;

namespace Vertical.CommandLine.Binding.Pipeline;

internal sealed class AddModelBindingValuesTasks : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        InvokeCore(context);
        next(context);
    }

    private static void InvokeCore(IBindingContext context)
    {
        IMappedArgumentProvider? argumentProvider = null;

        var binders = context
            .InvocationPath
            .SelectMany(command => command.ModelBinders);
        
        foreach (var binder in binders)
        {
            argumentProvider ??= context.CreateArgumentProvider();

            var modelValue = binder.BindInstanceBase(argumentProvider);
            context.AddModelValue(modelValue);
        }
    }
}