namespace Vertical.CommandLine.Validation;

public interface IValidationBuilder<out T>
{
    IValidationBuilder<T> Must(Func<T, bool> predicate, Func<T, string>? messageProvider = null);
}