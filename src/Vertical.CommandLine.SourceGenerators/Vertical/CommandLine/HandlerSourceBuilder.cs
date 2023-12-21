using System.Text;

namespace Vertical.CommandLine;

internal static class HandlerSourceBuilder
{
    public static string Build(HandlerMetadata[] handlers, bool hasDiagnostics)
    {
        var builder = new StringBuilder(25000);
        var cs = new CSharpFormatter(builder);

        if (handlers.Length > 0 && !hasDiagnostics)
            BuildSource(handlers, cs);
        
        return builder.ToString();
    }

    private static void BuildSource(HandlerMetadata[] handlers, CSharpFormatter cs)
    {
        cs.WriteHeader();
        cs.WriteUsingStatements(
            "System",
            "System.Collections.Generic",
            "System.Threading.Tasks",
            "Vertical.CommandLine",
            "Vertical.CommandLine.Binding",
            "Vertical.CommandLine.Invocation");
        cs.WriteNullableEnable();
        WriteExtensions(handlers, cs);
    }

    private static void WriteExtensions(HandlerMetadata[] handlers, CSharpFormatter cs)
    {
        cs.AppendLine($"namespace {Constants.BaseNamespace}");
        cs.AppendBlock(inner => WriteExtensionClass(handlers, inner));
    }

    private static void WriteExtensionClass(HandlerMetadata[] handlers, CSharpFormatter cs)
    {
        cs.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
        cs.AppendLine("public static class InvocationExtensions");
        cs.AppendBlock(inner => WriteClassBody(handlers, inner));
    }

    private static void WriteClassBody(HandlerMetadata[] handlers, CSharpFormatter cs)
    {
        var namingManager = new NamingManager();
        
        WriteInvokeExtensionMethod(handlers, cs, namingManager);
        WriteCommandInvocationMethods(handlers, cs, namingManager);
    }

    private static void WriteIndentedCsv(IReadOnlyList<string> items, CSharpFormatter cs)
    {
        if (items.Count > 0)
        {
            cs.Append(items[0]);
        }
        
        cs.AppendIndented(inner =>
        {
            foreach (var item in items.Skip(1))
            {
                inner.AppendLine(",");
                inner.Append(item);
            }
        });
    }

    private static void WriteInvokeExtensionMethod(
        HandlerMetadata[] handlers,
        CSharpFormatter cs,
        NamingManager namingManager)
    {
        // Already checked there is only 1 distinct return type
        var template = handlers.First();

        cs.Append("public static ");

        if (template.IsAsyncTask)
            cs.Append("async ");

        cs.Append(template.ReturnTypeDeclaration);
        cs.AppendLine(template.IsAsyncTask ? " InvokeAsync(" : " Invoke(");
        cs.AppendIndented(inner =>
        {
            inner.AppendLine("this RootCommand rootCommand,");
            inner.Append("IEnumerable<string> args");
            if (template.IsAsyncTask)
            {
                inner.AppendLine(",");
                inner.AppendLine("CancellationToken cancellationToken)");
                return;
            }
            inner.AppendLine(")");
        });
        cs.AppendBlock(inner => WriteExtensionMethodBody(handlers, inner, namingManager));
    }

    private static void WriteExtensionMethodBody(
        HandlerMetadata[] handlers,
        CSharpFormatter cs,
        NamingManager namingManager)
    {
        cs.Append("var context = InvocationContextBuilder.Create(rootCommand, args");
        cs.AppendLine(handlers[0].IsAsyncTask ? ", cancellationToken);" : ");");
        cs.AppendLine();
        cs.AppendLine("switch (context.CommandId)");
        cs.AppendBlock(inner => WriteExtensionSwitchCases(handlers, inner, namingManager));
    }

    private static void WriteExtensionSwitchCases(
        IEnumerable<HandlerMetadata> handlers,
        CSharpFormatter cs,
        NamingManager namingManager)
    {
        foreach (var handler in handlers)
        {
            cs.AppendLine($"case \"{handler.CommandId}\":");
            cs.AppendIndented(inner => WriteExtensionSwitchCase(handler, inner, namingManager));
        }
        cs.AppendLine("default:");
        cs.AppendIndented(inner =>
        {
            inner.AppendLine("throw new InvalidOperationException();");
        });
    }

    private static void WriteExtensionSwitchCase(
        HandlerMetadata handler,
        CSharpFormatter cs,
        NamingManager namingManager)
    {
        if (handler.ReturnsValue)
            cs.Append("return ");
        

        var handlerIdentifier = namingManager.GetHandlerClrCompliantName(handler);
        
        cs.AppendLine(handler.IsAsyncTask
            ? $"await Invoke{handlerIdentifier}CommandAsync(context);"
            : $"Invoke{handlerIdentifier}Command(context);");

        if (!handler.ReturnsValue)
            cs.AppendLine("break;");
    }

    private static void WriteCommandInvocationMethods(
        IEnumerable<HandlerMetadata> handlers,
        CSharpFormatter cs,
        NamingManager namingManager)
    {
        foreach (var handler in handlers)
        {
            cs.AppendLine();
            WriteCommandInvocationMethod(handler, cs, namingManager);
        }
    }

    private static void WriteCommandInvocationMethod(
        HandlerMetadata handler,
        CSharpFormatter cs,
        NamingManager namingManager)
    {
        cs.Append("private static ");
        if (handler.IsAsyncTask)
            cs.Append("async ");
        cs.Append(handler.ReturnTypeDeclaration);
        cs.Append($" Invoke{namingManager.GetHandlerClrCompliantName(handler)}Command");
        cs.AppendLine(handler.IsAsyncTask ? "Async(" : "(");
        cs.AppendIndented(inner =>
        {
            inner.AppendLine("IInvocationContext context)");
        });
        cs.AppendBlock(inner => WriteCommandInvocationMethodBody(handler, inner));
    }

    private static void WriteCommandInvocationMethodBody(HandlerMetadata handler, CSharpFormatter cs)
    {
        foreach (var parameter in handler.Parameters)
        {
            cs.Append($"var {parameter.Name} = ");
            if (parameter.IsCancellationToken)
            {
                cs.AppendLine("context.CancellationToken;");
                continue;
            }

            var argumentCall = Utilities.FormatArgumentProviderCall(parameter);
            cs.AppendLine($"context.ArgumentProvider.{argumentCall};");
        }
        cs.AppendLine();

        var symbols = handler
            .Parameters
            .Select(param => param.Type)
            .ToList();
        var isFunction = symbols.Any();

        if (handler.ReturnsValue || handler.IsAsyncTask)
        {
            symbols.Add(handler.ReturnType);
        }
        
        cs.Append("var callSite = (");
        cs.Append(isFunction ? "Func<" : "Action");
        
        WriteIndentedCsv(symbols.Select(symbol => symbol.ToDisplayString()).ToArray(), cs);
        cs.Append(isFunction ? ">)" : ")");
        cs.AppendLine("context.Handler!;");
        cs.AppendLine();

        if (handler.ReturnsValue)
            cs.Append("return ");
        
        if (handler.IsAsyncTask)
            cs.Append("await ");
        
        cs.Append("callSite(");
        WriteIndentedCsv(handler.Parameters.Select(param => param.Name).ToArray(), cs);
        cs.AppendLine(");");
    }
}