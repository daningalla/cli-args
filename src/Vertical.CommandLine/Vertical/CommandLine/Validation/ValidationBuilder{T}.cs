namespace Vertical.CommandLine.Validation;

public sealed class ValidatorBuilder<T>
{
    private readonly List<ValidationRule<T>> _rules = new(4);
    
    public ValidatorBuilder<T> Must(Func<T, bool> predicate, Func<T, string>? messageProvider = null)
    {
        _rules.Add(new ValidationRule<T>(context => predicate(context.AttemptedValue), messageProvider));
        return this;
    }
    
    public ValidatorBuilder<T> MustSatisfy(ValidationRule<T> rule)
    {
        _rules.Add(rule);
        return this;
    }

    internal IValidator<T> Build() => new RuleBasedValidator<T>(_rules);
}