using Vertical.CommandLine.Invocation;
using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Binding;

internal static class BindingContextExtensions
{
    internal static IInvocationContext GetInvocationContext(this IBindingContext context)
    {
        var subject = context.InvocationSubject;
        var argumentProvider = new MappedArgumentProvider(
            context.Services,
            context.GetValueBindings());

        if (subject.Handler == null)
        {
            // Application arguments did not match a valid command
            throw new CommandLineInvocationException(subject);
        }

        return new InvocationContext(
            context.InvocationSubject.Handler ?? throw ConfigurationExceptions.NoCommandHandler(subject),
            subject.Id,
            argumentProvider,
            context.CancellationToken
            );
    }
}