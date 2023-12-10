using System.Text;
using Microsoft.CodeAnalysis;

namespace Vertical.CommandLine;

internal static class Diagnostics
{
    internal static Diagnostic Vcl_001(SyntaxNode syntaxNode)
    {
        return Diagnostic.Create(
            new DiagnosticDescriptor(
                "vcl-001",
                "Command id not supported.",
                "Command identifiers must be non-null, non-empty compile-time string constants - sources will not be generated.",
                "Design",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true),
            syntaxNode.GetLocation());
    }

    internal static Diagnostic Vcl_002()
    {
        return Diagnostic.Create(
            new DiagnosticDescriptor(
                "vcl-002",
                "Multiple root commands not supported",
                "An application can only define a single root command for handler source generation - " + "" +
                "sources will not be generated.",
                "Design",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true),
            location: null);
    }

    internal static Diagnostic Vcl_003(IEnumerable<HandlerMetadata> handlers)
    {
        var sb = new StringBuilder(500);
        
        sb.AppendLine("Command handler (including the root command) delegates/methods all must return the same type " +
                      "- sources will not be generated.");
        sb.AppendLine("Found the following:");
        foreach (var handler in handlers)
        {
            var description = handler.IsRootCommand ? "RootCommand" : "Command";
            sb.AppendLine($"-> {description} '{handler.CommandId}' -> {handler.ReturnType.ToDisplayString()}");
        }

        return Diagnostic.Create(
            new DiagnosticDescriptor(
                "vcl-003",
                "Multiple command handler return types.",
                sb.ToString(),
                "Design",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true),
            location: null);
    }

    internal static Diagnostic Vcl_004(IEnumerable<string> commandIds)
    {
        var sb = new StringBuilder(500);

        sb.AppendLine("Command identifiers must be unique - sources will not be generated.");
        sb.AppendLine("Found the following non-unique ids:");
        
        foreach (var id in commandIds)
        {
            sb.AppendLine($"> '{id}'");
        }

        return Diagnostic.Create(
            new DiagnosticDescriptor(
                "vcl-004",
                "Repeated command ids.",
                sb.ToString(),
                "Design",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true),
            location: null);
    }
}