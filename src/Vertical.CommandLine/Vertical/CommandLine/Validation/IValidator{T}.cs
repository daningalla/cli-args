namespace Vertical.CommandLine.Validation;

public interface IValidator<T> : IValidator
{
    void Validate(IValidationContext<T> context);
}