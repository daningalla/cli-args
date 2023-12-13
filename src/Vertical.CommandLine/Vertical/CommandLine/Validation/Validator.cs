namespace Vertical.CommandLine.Validation;

public static class Validator
{
    public static IValidator<T> Build<T>(Action<ValidatorBuilder<T>> configure)
    {
        var builder = new ValidatorBuilder<T>();
        configure(builder);

        return builder.Build();
    }

    internal static IValidator<T>? TryBuild<T>(this Action<ValidatorBuilder<T>>? action)
    {
        return action != null ? Build(action) : null;
    }
}