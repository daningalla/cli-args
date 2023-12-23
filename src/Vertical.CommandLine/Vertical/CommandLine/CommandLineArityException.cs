using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine;

/// <summary>
/// Indicates a condition in which an arity was not met or was exceeded.
/// </summary>
public class CommandLineArityException : CommandLineException
{
    internal CommandLineArityException(CliBindingSymbol bindingSymbol, IEnumerable<string?> arguments)
        : base(FormatMessage(bindingSymbol, arguments))
    {
        BindingSymbol = bindingSymbol;
        Arguments = arguments;
    }

    /// <summary>
    /// Gets the binding symbol.
    /// </summary>
    public CliBindingSymbol BindingSymbol { get; }

    /// <summary>
    /// Gets the arguments.
    /// </summary>
    public IEnumerable<string?> Arguments { get; }

    private static string FormatMessage(CliBindingSymbol bindingSymbol, IEnumerable<string?> arguments)
    {
        return ReusableStringBuilder.Build(sb =>
        {
            var argumentArray = arguments.Where(arg => arg is not null).ToArray();

            if (argumentArray.Length < bindingSymbol.Arity.MinCount)
            {
                sb.Append($"{bindingSymbol.SymbolType} '{bindingSymbol}' expected at least " +
                          "{bindingSymbol.Arity.MinCount} arguments, ");
                
                if (argumentArray.Length == 0)
                {
                    sb.AppendLine("but none were received.");
                    return;
                }

                sb.AppendLine("but only received the following:");
                foreach (var arg in argumentArray)
                {
                    sb.AppendLine($"\t\"{arg}\"");
                }
                return;
            }
            
            sb.Append($"{bindingSymbol.SymbolType} '{bindingSymbol}' expected no more than " + 
                      "{bindingSymbol.Arity.MaxCount} use, ");

            if (bindingSymbol.SymbolType == CliSymbolType.Switch)
            {
                sb.AppendLine($"but {argumentArray.Length} were received.");
                return;
            }
            
            sb.AppendLine("but received the following:");
            foreach (var arg in argumentArray)
            {
                sb.AppendLine($"\t\"{arg}\"");
            }
        });
    }
}