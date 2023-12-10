namespace Vertical.CommandLine.Validation;

public interface IValidator<T> : IValidator
{
    bool Validate(ValidationContext<T> context);
    
    Func<ValidationContext<T>, string>? MessageFormatter { get; }
}