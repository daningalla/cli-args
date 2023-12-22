using CommunityToolkit.Diagnostics;

namespace Vertical.CommandLine.Validation;

/// <summary>
/// Defines method used to construct validation chains.
/// </summary>
/// <typeparam name="T">Value type subject to validation.</typeparam>
public sealed class ValidatorBuilder<T>
{
    private readonly List<ValidationRule<T>> _rules = new(4);
    
    /// <summary>
    /// Adds a validation rule.
    /// </summary>
    /// <param name="rule">The rule to add.</param>
    /// <returns>A reference to this instance.</returns>
    public ValidatorBuilder<T> MustSatisfy(ValidationRule<T> rule)
    {
        Guard.IsNotNull(rule);
        
        _rules.Add(rule);
        return this;
    }

    internal IValidator<T> Build() => new RuleBasedValidator<T>(_rules);
}