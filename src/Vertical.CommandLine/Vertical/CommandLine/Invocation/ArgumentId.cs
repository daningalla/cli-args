namespace Vertical.CommandLine.Invocation;

/// <summary>
/// Represents an argument id, used in the <see cref="MappedArgumentProvider"/> matching
/// operations.
/// </summary>
/// <param name="Id">The id to map.</param>
public readonly record struct ArgumentId(string Id);