using System.Text;
using Microsoft.CodeAnalysis;

namespace Vertical.CommandLine;

internal static class GeneratedBindingSourceBuilder
{
    public static string Build(GeneratedBindingMetadata metadata)
    {
        var builder = new StringBuilder(25000);
        var cs = new CSharpFormatter(builder);

        cs.WriteHeader();
        cs.WriteUsingStatements(
            "System",
            "System.Collections.Generic",
            "Vertical.CommandLine.Binding",
            "Vertical.CommandLine.Invocation");
        cs.WriteNullableEnable();

        WriteBinderImplementation(cs, metadata);
        
        return builder.ToString();
    }

    private static void WriteBinderImplementation(CSharpFormatter cs, GeneratedBindingMetadata metadata)
    {
        cs.AppendLine($"namespace {metadata.BinderTypeNamespace}");
        cs.AppendBlock(inner => WriteClassDeclaration(inner, metadata));
    }

    private static void WriteClassDeclaration(CSharpFormatter cs, GeneratedBindingMetadata metadata)
    {
        var modelType = metadata.ModelTypeSymbol;
        var modelTypeName = $"{modelType.ContainingNamespace.ToDisplayString()}.{modelType.Name}";
        var baseTypeName = $"ModelBinder<{modelTypeName}>";
        
        cs.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
        cs.AppendLine($"public partial class {metadata.BinderTypeName} : {baseTypeName}");
        cs.AppendBlock(inner => WriteClassBody(inner, metadata, modelTypeName));
    }

    private static void WriteClassBody(CSharpFormatter cs, GeneratedBindingMetadata metadata, string modelTypeName)
    {
        cs.AppendLine($"protected override {modelTypeName} BindInstance(IMappedArgumentProvider argumentProvider)");
        cs.AppendBlock(inner => WriteBindInstanceBody(inner, metadata, modelTypeName));
    }

    private static void WriteBindInstanceBody(
        CSharpFormatter cs,
        GeneratedBindingMetadata metadata,
        string modelTypeName)
    {
        var typeSymbol = (INamedTypeSymbol)metadata.ModelTypeSymbol;
        var constructorParams = typeSymbol
            .Constructors
            .Single(constructor => !IsRecordCopyConstructor(constructor))
            .Parameters
            .Select(parameter => new ParameterMetadata(parameter))
            .ToArray();
        var handledParameters = new HashSet<string>();
        
        cs.Append($"var instance = new {modelTypeName}(");

        if (constructorParams.Length > 0)
        {
            cs.AppendLine();
            cs.AppendIndented(inner =>
            {
                foreach (var (parameter, i) in constructorParams.Select((p, i) => (p,i)))
                {
                    if (i > 0) inner.AppendLine(",");
                    inner.Append($"{parameter.Name}: ");
                    inner.Append($"argumentProvider.{Utilities.FormatArgumentProviderCall(parameter)}");
                    handledParameters.Add(parameter.Name);
                }
            });
        }
        cs.Append(")");

        var properties = typeSymbol
            .GetMembers()
            .Where(member => member is IPropertySymbol { IsReadOnly: false } && handledParameters.Add(member.Name))
            .Cast<IPropertySymbol>()
            .Select(property => new PropertyMetadata(property))
            .ToArray();

        if (properties.Length == 0)
        {
            cs.AppendLine(";");
        }
        else
        {
            cs.AppendLine();
            cs.AppendBlock("{", "};", inner =>
            {
                foreach (var (property, i) in properties.Select((p, i) => (p, i)))
                {
                    if (i > 0) inner.AppendLine(",");
                    inner.Append($"{property.Name} = ");
                    inner.Append($"argumentProvider.{Utilities.FormatArgumentProviderCall(property)}");
                }
            });
        }

        cs.AppendLine();
        cs.AppendLine($"return instance;");
    }

    private static bool IsRecordCopyConstructor(IMethodSymbol constructor)
    {
        return constructor.Parameters.Length == 1 &&
               SymbolEqualityComparer.Default.Equals(
                   constructor.Parameters[0].Type, 
                   constructor.ContainingType);
    }
}