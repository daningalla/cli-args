using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine;

/// <summary>
/// Indicates an invocation exception occurred (command not matched).
/// </summary>
public class CommandLineInvocationException : CommandLineException
{
    internal CommandLineInvocationException(Command subject)
        : base(FormatMessage(subject))
    {
    }

    private static string FormatMessage(Command subject)
    {
        return ReusableStringBuilder.Build(sb =>
        {
            sb.AppendLine("Invalid command. Expected one of the following:");
            
            foreach (var command in subject.Commands.OrderBy(cmd => cmd.Id))
            {
                var identifierString = string.Join(", ", command.Identifiers);
                sb.AppendLine($"  > {identifierString}");
            }
        });
    }
}