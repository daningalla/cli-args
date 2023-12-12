namespace Vertical.CommandLine.Validation;

public interface IValidatorBuilder<out T>
{
    IValidatorBuilder<T> Must(Func<T, bool> predicate, Func<T, string>? messageProvider = null);
}