namespace Vertical.CommandLine;

/// <summary>
/// Represents a condition that results from errors found in argument input (not configuration).
/// </summary>
public abstract class CommandLineException : Exception
{
    protected CommandLineException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}