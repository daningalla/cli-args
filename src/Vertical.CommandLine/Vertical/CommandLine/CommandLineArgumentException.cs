using Vertical.CommandLine.Syntax;

namespace Vertical.CommandLine;

/// <summary>
/// Indicates an invalid argument was received.
/// </summary>
public class CommandLineArgumentException : CommandLineException
{
    internal CommandLineArgumentException(SemanticArgument argument)
        : base(FormatMessage(argument))
    {
        Argument = argument;
    }

    /// <summary>
    /// Gets the argument.
    /// </summary>
    public SemanticArgument Argument { get; }

    private static string FormatMessage(SemanticArgument argument)
    {
        return argument.Anatomy.PrefixFormat != IdentifierFormat.None 
            ? $"Unknown option or switch {argument.Text}." 
            : $"Invalid argument {argument}";
    }
}