namespace Vertical.CommandLine.Binding.Pipeline;

internal class PostValidateContextTask : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        InvokeCore(context);
        next(context);
    }

    private static void InvokeCore(IBindingContext context)
    {
        var remainingArguments = context.GetSemanticArguments();

        if (remainingArguments.IsEmpty)
            return;

        var first = remainingArguments.First();

        throw new CommandLineArgumentException(first);
    }
}