using Microsoft.CodeAnalysis;

namespace Vertical.CommandLine;

public sealed class HandlerMetadata
{
    public HandlerMetadata(
        string commandId,
        bool isRootCommand,
        IMethodSymbol handlerMethod,
        ParameterMetadata[] parameters)
    {
        CommandId = commandId;
        IsRootCommand = isRootCommand;
        HandlerMethod = handlerMethod;
        Parameters = parameters;
        ReturnType = handlerMethod.ReturnType;
        ReturnsValue = GetReturnsValue(handlerMethod.ReturnType);
        IsAsyncTask = GetIsAsyncTask(handlerMethod.ReturnType);
        ReturnTypeDeclaration = GetReturnTypeDeclaration(ReturnType);
    }
    
    public Guid Id { get; } = Guid.NewGuid();

    public string ReturnTypeDeclaration { get; set; }

    public bool ReturnsValue { get; set; }

    public bool IsAsyncTask { get; set; }

    public ITypeSymbol ReturnType { get; set; }

    public string CommandId { get; }

    public bool IsRootCommand { get; }

    public IMethodSymbol HandlerMethod { get; }

    public ParameterMetadata[] Parameters { get; }

    private static bool GetIsAsyncTask(ISymbol returnTypeSymbol)
    {
        return returnTypeSymbol.ToDisplayString().StartsWith("System.Threading.Tasks.Task");
    }

    private static bool GetReturnsValue(ISymbol returnTypeSymbol)
    {
        return returnTypeSymbol.ToDisplayString() is not "System.Void" and not "System.Threading.Tasks.Task";
    }

    private static string GetReturnTypeDeclaration(ITypeSymbol symbol)
    {
        var displayString = symbol.ToDisplayString();
        if (!displayString.StartsWith("System.Threading.Tasks.Task<"))
            return displayString;

        var genericType = symbol.GetGenericArgumentType();
        
        return genericType != null
            ? $"Task<{genericType.ToDisplayString()}>"
            : displayString;
    }
}