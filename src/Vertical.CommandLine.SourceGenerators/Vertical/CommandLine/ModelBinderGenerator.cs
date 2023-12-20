using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Vertical.CommandLine;

[Generator]
public class ModelBinderGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var metadataProvider = context
            .SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, ctx) => IsClassDeclarationWithGeneratedAttributeSyntax(node, ctx),
                transform: static (syntaxContext, ctx) => TransformSyntaxMetadata(syntaxContext, ctx))
            .Where(x => x is not null)
            .WithComparer(GeneratedBindingMetadata.Comparer);
        
        context.RegisterSourceOutput(metadataProvider, (productionContext, metadata) =>
        {
            if (metadata == null)
                return;
            
            var sourceContent = GeneratedBindingSourceBuilder.Build(metadata);
            var fileName = $"{metadata.BinderTypeName}.g.cs";
            productionContext.AddSource(fileName, sourceContent);
        });
    }

    private static bool IsClassDeclarationWithGeneratedAttributeSyntax(SyntaxNode node, CancellationToken ctx)
    {
        ctx.ThrowIfCancellationRequested();
        
        return node is ClassDeclarationSyntax { AttributeLists.Count: > 0 } classDeclarationSyntax
               && classDeclarationSyntax
                   .AttributeLists
                   .Any(list => list
                       .Attributes
                       .Any(AttributeIsGeneratedBindingAttribute));
    }

    private static GeneratedBindingMetadata? TransformSyntaxMetadata(
        GeneratorSyntaxContext syntaxContext,
        CancellationToken ctx)
    {
        // We don't care about the model declaration at this point, we just want it to trigger the generator
        var classDeclaration = (ClassDeclarationSyntax)syntaxContext.Node;
        var genericBindingAttribute = GetGenericGeneratedBindingAttribute(classDeclaration);

        if (genericBindingAttribute == null)
            return null;
        
        ctx.ThrowIfCancellationRequested();
        
        // Get the model type referenced by the attribute in the class declaration
        var genericName = (GenericNameSyntax)genericBindingAttribute.Name;
        var typeArgument = genericName
            .TypeArgumentList
            .Arguments
            .FirstOrDefault();

        if (typeArgument == null)
            return null;

        var modelSymbol = syntaxContext
            .SemanticModel
            .GetSymbolInfo(typeArgument);

        if (modelSymbol.Symbol == null)
            return null;
        
        // Traverse back to compilation unit
        SyntaxNode? node = classDeclaration;
        for (; node is not CompilationUnitSyntax; node = node?.Parent)
        {
        }

        var namespaceNode = node
            .ChildNodes()
            .FirstOrDefault(child => child is BaseNamespaceDeclarationSyntax) as BaseNamespaceDeclarationSyntax;

        var namespaceName = namespaceNode?.Name.ToString();
        var typeName = classDeclaration.Identifier.ToString();

        return new GeneratedBindingMetadata(namespaceName, typeName, modelSymbol.Symbol);
    }

    private static bool AttributeIsGeneratedBindingAttribute(AttributeSyntax attribute)
    {
        var identifier = attribute.Name switch
        {
            SimpleNameSyntax simpleNameSyntax => simpleNameSyntax.Identifier.Text,
            QualifiedNameSyntax qualifiedNameSyntax => qualifiedNameSyntax.Right.Identifier.Text,
            _ => null
        };

        return identifier is "GeneratedBinding" or "GeneratedBindingAttribute";
    }

    private static AttributeSyntax? GetGenericGeneratedBindingAttribute(MemberDeclarationSyntax classDeclarationSyntax)
    {
        var attributes = classDeclarationSyntax.AttributeLists.SelectMany(list => list.Attributes);

        return attributes
            .FirstOrDefault(attribute => AttributeIsGeneratedBindingAttribute(attribute)
                                         && attribute.Name is GenericNameSyntax);
    }
}