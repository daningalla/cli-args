using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Binding.Pipeline;

public class PostValidateContextTask : IBindingTask
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

        throw CommandLineException.InvalidArgument(first);
    }
}