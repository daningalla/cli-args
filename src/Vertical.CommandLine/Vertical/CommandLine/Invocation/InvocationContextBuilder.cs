using CommunityToolkit.Diagnostics;
using Vertical.CommandLine.Binding;
using Vertical.CommandLine.Binding.Pipeline;

namespace Vertical.CommandLine.Invocation;

/// <summary>
/// Creates an invocation context.
/// </summary>
public static class InvocationContextBuilder
{
    /// <summary>
    /// Creates an invocation context using the application's root command definition and supplied
    /// arguments.
    /// </summary>
    /// <param name="rootCommand">The root command definition.</param>
    /// <param name="args">Application arguments.</param>
    /// <param name="cancellationToken">A token that can be observed for cancellation requests.</param>
    /// <returns><see cref="IInvocationContext"/></returns>
    public static IInvocationContext Create(
        RootCommand rootCommand, 
        IEnumerable<string> args,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(rootCommand);
        Guard.IsNotNull(args);
        
        var bindingContext = BindingPipeline.CreateContext(rootCommand, args, cancellationToken);
        var invocationContext = bindingContext.GetInvocationContext();

        return invocationContext;
    }
}