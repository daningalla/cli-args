namespace Vertical.CommandLine.Invocation;

/// <summary>
/// Represents a context that describes the invocation of a command.
/// </summary>
public interface IInvocationContext
{
    /// <summary>
    /// Gets the handler to invoke that contains the application's logic for the selected command.
    /// </summary>
    Delegate? Handler { get; }
    
    /// <summary>
    /// Gets the command id.
    /// </summary>
    string CommandId { get; }
    
    /// <summary>
    /// Gets a service that provides argument values mapped from the application user's input.
    /// </summary>
    IMappedArgumentProvider ArgumentProvider { get; }
    
    /// <summary>
    /// Gets the cancellation token to use during asynchronous invocations.
    /// </summary>
    CancellationToken CancellationToken { get; }
}