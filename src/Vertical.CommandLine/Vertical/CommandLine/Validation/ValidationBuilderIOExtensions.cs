using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace Vertical.CommandLine.Validation;

public static class ValidationBuilderIOExtensions
{
    /// <summary>
    /// Adds a rule that passes validation if the string version of the value references a file that exists.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    [ExcludeFromCodeCoverage]
    public static ValidatorBuilder<FileInfo> FileExists(
        this ValidatorBuilder<FileInfo> builder,
        Func<FileInfo, string>? messageFormatter = null)
    {
        Guard.IsNotNull(builder);
        
        return builder.MustSatisfy(new ValidationRule<FileInfo>(
            context => context.AttemptedValue.Exists,
            messageFormatter ?? (file => $"File not found {file.FullName}.")));
    }
    
    /// <summary>
    /// Adds a rule that passes validation if the string version of the value references a directory that exists.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    [ExcludeFromCodeCoverage]
    public static ValidatorBuilder<DirectoryInfo> DirectoryExists(
        this ValidatorBuilder<DirectoryInfo> builder,
        Func<DirectoryInfo, string>? messageFormatter = null)
    {
        Guard.IsNotNull(builder);
        
        return builder.MustSatisfy(new ValidationRule<DirectoryInfo>(
            context => context.AttemptedValue.Exists,
            messageFormatter ?? (dir => $"Directory not found {dir.FullName}.")));
    }
}