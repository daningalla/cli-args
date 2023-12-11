namespace Vertical.CommandLine.Validation;

public record ValidationRuleImplementation<T>(
    Func<ValidationContext<T>, bool> Predicate,
    Func<ValidationContext<T>, string>? MessageProvider);