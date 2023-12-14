namespace Vertical.CommandLine.Binding.Pipeline;

public sealed class AddValidatorServicesTask : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        InvokeCore(context);
        next(context);
    }

    private static void InvokeCore(IBindingContext context)
    {
        context.Services.AddRange(context
            .InvocationPath
            .SelectMany(command => command.Validators));
    }
}