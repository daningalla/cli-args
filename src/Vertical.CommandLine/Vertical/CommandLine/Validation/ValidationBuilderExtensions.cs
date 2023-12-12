namespace Vertical.CommandLine.Validation;

public static class ValidationBuilderExtensions
{
    public static IValidatorBuilder<string?> MinimumLength(
        this IValidatorBuilder<string?> builder,
        int length)
    {
        return builder.Must(str => str?.Length >= length);
    }
}