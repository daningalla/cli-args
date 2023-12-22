namespace Vertical.CommandLine.Invocation;

internal sealed class InvocationContext : IInvocationContext
{
    internal InvocationContext(
        Delegate handler,
        string commandId,
        IMappedArgumentProvider argumentProvider,
        CancellationToken cancellationToken)
    {
        Handler = handler;
        CommandId = commandId;
        ArgumentProvider = argumentProvider;
        CancellationToken = cancellationToken;
    }
    
    /// <inheritdoc />
    public Delegate Handler { get; }

    /// <inheritdoc />
    public string CommandId { get; }

    /// <inheritdoc />
    public IMappedArgumentProvider ArgumentProvider { get; }

    /// <inheritdoc />
    public CancellationToken CancellationToken { get; }
}