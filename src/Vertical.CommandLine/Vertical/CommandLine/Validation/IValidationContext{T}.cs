namespace Vertical.CommandLine.Validation;

/// <summary>
/// Represents a validation context.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public interface IValidationContext<T>
{
    /// <summary>
    /// Gets the value subject to validation.
    /// </summary>
    T AttemptedValue { get; }

    /// <summary>
    /// Gets the collection of failures.
    /// </summary>
    ICollection<ValidationRule<T>> Failures { get; }
    
    /// <summary>
    /// Gets whether <see cref="AttemptedValue"/> is valid, e.g. functionally if
    /// no violations have been added.
    /// </summary>
    bool IsValid { get; }
}