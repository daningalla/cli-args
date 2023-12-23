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

    public string ReturnTypeDeclaration { get; }

    public bool ReturnsValue { get; }

    public bool IsAsyncTask { get; }

    public ITypeSymbol ReturnType { get; }

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
        switch (returnTypeSymbol.ToDisplayString())
        {
            case "void":
                return false;
            case "System.Threading.Tasks.Task":
                return false;
            default:
                return true;
        }
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