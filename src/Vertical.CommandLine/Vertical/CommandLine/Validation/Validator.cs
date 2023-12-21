namespace Vertical.CommandLine.Validation;

public static class Validator
{
    public static IValidator<T> Build<T>(Action<ValidatorBuilder<T>> configure)
    {
        var builder = new ValidatorBuilder<T>();
        configure(builder);

        return builder.Build();
    }
}