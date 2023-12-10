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
        
        return new InvocationContext
        {
            Handler = context.InvocationSubject.Handler ?? throw ConfigurationExceptions.NoCommandHandler(subject),
            ArgumentProvider = argumentProvider,
            CancellationToken = context.CancellationToken,
            CommandId = subject.Id
        };
    }
}