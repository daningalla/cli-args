using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Syntax;

namespace Vertical.CommandLine.Binding.Pipeline;

internal sealed class BuildCommandPathTask : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        InvokeCore(context);
        next(context);
    }

    private static void InvokeCore(IBindingContext context)
    {
        Command command = context.RootCommand;
        var queue = new Queue<string>(context.ArgumentValues);

        while (true)
        {
            context.AddInvocation(command);
            
            if (command.Commands.Count == 0)
                break;

            if (!queue.TryPeek(out var arg))
                break;

            if (arg == SyntaxConstants.Terminator)
                break;

            var child = command.Commands.FirstOrDefault(cmd => cmd.IsMatchToAnyIdentifier(arg));

            if (child == null)
                break;

            queue.Dequeue();
            command = child;
        }
        
        context.AddInvocationArgumentValues(queue);
    }
}