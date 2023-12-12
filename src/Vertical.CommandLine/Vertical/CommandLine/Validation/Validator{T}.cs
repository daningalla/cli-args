namespace Vertical.CommandLine.Validation;

public sealed class Validator<T> : IValidator<T>
{
    private readonly IEnumerable<ValueConstraint<T>> _constraints;

    public Validator(IEnumerable<ValueConstraint<T>> constraints)
    {
        _constraints = constraints;
    }
    
    /// <inheritdoc />
    public void Validate(IValidationContext<T> context)
    {
        foreach (var constraint in _constraints.Where(c => !c.IsValid(context.AttemptedValue)))
        {
            context.Failures.Add(constraint);
        }
    }

    /// <inheritdoc />
    public Type ServiceType => typeof(IValidator<T>);
}