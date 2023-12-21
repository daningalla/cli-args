namespace Vertical.CommandLine.Validation;

/// <summary>
/// Base class for validator implementations.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public abstract class Validator<T> : IValidator<T>
{
    /// <inheritdoc />
    public void Validate(IValidationContext<T> context)
    {
        var result = Validate(context.AttemptedValue);

        if (result.IsValid)
            return;
        
        context.Failures.Add(new ValidationRule<T>(
            _ => false,
            _ => result.Message));
    }

    /// <inheritdoc />
    public Type ServiceType => typeof(IValidator<T>);

    /// <summary>
    /// When implemented by a class, determines if <paramref name="value"/> is valid.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns><c>true</c> if <paramref name="value"/> is valid, otherwise message describing the invalid state.</returns>
    protected abstract ValidationResult Validate(T value);
}