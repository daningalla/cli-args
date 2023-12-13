namespace Vertical.CommandLine.Binding.Pipeline;

public class AddCommandPathServicesTask : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        InvokeCore(context);
        next(context);
    }

    private static void InvokeCore(IBindingContext context)
    {
        foreach (var command in context.InvocationPath)
        {
            context.Services.AddRange(command.Converters);
            context.Services.AddRange(command.Validators);
        }
    }
}