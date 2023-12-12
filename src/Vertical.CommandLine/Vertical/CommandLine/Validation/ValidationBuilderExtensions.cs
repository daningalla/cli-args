namespace Vertical.CommandLine.Validation;

public static class ValidationBuilderExtensions
{
    public static ValidatorBuilder<string?> MinimumLength(
        this ValidatorBuilder<string?> builder,
        int length,
        Func<string?, string>? messageFormatter)
    {
        return builder.Must(str => str?.Length >= length, messageFormatter);
    }
}