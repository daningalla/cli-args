namespace Vertical.CommandLine.Invocation;

internal sealed class InvocationContext : IInvocationContext
{
    /// <inheritdoc />
    public required Delegate Handler { get; init;  }

    /// <inheritdoc />
    public required string CommandId { get; init; }

    /// <inheritdoc />
    public required IMappedArgumentProvider ArgumentProvider { get; init; }

    /// <inheritdoc />
    public required CancellationToken CancellationToken { get; init; }
}