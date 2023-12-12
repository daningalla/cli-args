namespace Vertical.CommandLine.Validation;

/// <summary>
/// Represents a validator with a single value constraint.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public class ConstraintValidator<T> : IValidator<T>
{
    private readonly ValueConstraint<T> _constraint;

    public ConstraintValidator(Func<T, bool> predicate, Func<T, string>? messageFormatter = null)
        : this(new ValueConstraint<T>(predicate, messageFormatter))
    {
    }

    public ConstraintValidator(ValueConstraint<T> constraint)
    {
        _constraint = constraint;
    }

    /// <inheritdoc />
    public void Validate(IValidationContext<T> context)
    {
        if (_constraint.IsValid(context.AttemptedValue))
            return;
        
        context.Failures.Add(_constraint);
    }

    /// <inheritdoc />
    public Type ServiceType => typeof(IValidator<T>);
}