namespace Vertical.CommandLine.Validation;

public class RuleBasedValidator<T> : IValidator<T>
{
    private readonly IEnumerable<ValidationRule<T>> _rules;

    public RuleBasedValidator(IEnumerable<ValidationRule<T>> rules)
    {
        _rules = rules;
    }
    
    /// <inheritdoc />
    public void Validate(IValidationContext<T> context)
    {
        foreach (var rule in _rules)
        {
            if (rule.Validator(context))
                continue;
            
            context.Failures.Add(rule);
        }
    }

    /// <inheritdoc />
    public Type ServiceType => typeof(IValidator<T>);
}