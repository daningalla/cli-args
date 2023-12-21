namespace Vertical.CommandLine.Validation;

public static class ValidationBuilderComparableExtensions
{
    /// <summary>
    /// Adds a rule that passes validation if the attempted value is less than the specified value.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="value">The value to compare.</param>
    /// <param name="comparer">The comparer instance used to determine the relational position among values.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> LessThan<T>(
        this ValidatorBuilder<T> builder,
        T value, 
        IComparer<T>? comparer = null,
        Func<T, string>? messageFormatter = null)
        where T : IComparable<T>
    {
        return builder.MustSatisfy(new ValidationRule<T>(
                context => (comparer ?? Comparer<T>.Default).Compare(context.AttemptedValue, value) < 0,
                messageFormatter ?? (_ => $"Value must be less than {value}.")));
    }
    
    /// <summary>
    /// Adds a rule that passes validation if the attempted value is less than or equals the specified value.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="value">The value to compare.</param>
    /// <param name="comparer">The comparer instance used to determine the relational position among values.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> LessThanOrEquals<T>(
        this ValidatorBuilder<T> builder,
        T value, 
        IComparer<T>? comparer = null,
        Func<T, string>? messageFormatter = null)
        where T : IComparable<T>
    {
        return builder.MustSatisfy(new ValidationRule<T>(
            context => (comparer ?? Comparer<T>.Default).Compare(context.AttemptedValue, value) <= 0,
            messageFormatter ?? (_ => $"Value must be less than or equal to {value}.")));
    }
    
    /// <summary>
    /// Adds a rule that passes validation if the attempted value is greater than the specified value.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="value">The value to compare.</param>
    /// <param name="comparer">The comparer instance used to determine the relational position among values.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> GreaterThan<T>(
        this ValidatorBuilder<T> builder,
        T value, 
        IComparer<T>? comparer = default,
        Func<T, string>? messageFormatter = null)
        where T : IComparable<T>
    {
        return builder.MustSatisfy(new ValidationRule<T>(
            context => (comparer ?? Comparer<T>.Default).Compare(context.AttemptedValue, value) > 0,
            messageFormatter ?? (_ => $"Value must be greater than {value}.")));
    }
    
    /// <summary>
    /// Adds a rule that passes validation if the attempted value is greater than or equal to the specified value.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="value">The value to compare.</param>
    /// <param name="comparer">The comparer instance used to determine the relational position among values.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> GreaterThanOrEquals<T>(
        this ValidatorBuilder<T> builder,
        T value, 
        IComparer<T>? comparer = null,
        Func<T, string>? messageFormatter = null)
        where T : IComparable<T>
    {
        return builder.MustSatisfy(new ValidationRule<T>(
            context => (comparer ?? Comparer<T>.Default).Compare(context.AttemptedValue, value) >= 0,
            messageFormatter ?? (_ => $"Value must be greater than or equal to {value}.")));
    }

    /// <summary>
    /// Adds a rule that passes validation if the attempted value inclusively between the specified values.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="comparer">The comparer instance used to determine the relational position among values.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <param name="minimumValue">The inclusive minimum value.</param>
    /// <param name="maximumValue">The inclusive maximum value.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> InclusivelyBetween<T>(
        this ValidatorBuilder<T> builder,
        T minimumValue,
        T maximumValue,
        IComparer<T>? comparer = null,
        Func<T, string>? messageFormatter = null)
        where T : IComparable<T>
    {
        return builder.MustSatisfy(new ValidationRule<T>(
            context =>
            {
                comparer ??= Comparer<T>.Default;
                return comparer.Compare(context.AttemptedValue, minimumValue) >= 0 &&
                       comparer.Compare(context.AttemptedValue, maximumValue) <= 0;
            },
            messageFormatter ?? (_ => $"Value must be inclusively between {minimumValue} and {maximumValue}.")));
    }
    
    /// <summary>
    /// Adds a rule that passes validation if the attempted value exclusively between the specified values.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="comparer">The comparer instance used to determine the relational position among values.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <param name="minimumValue">The exclusive minimum value.</param>
    /// <param name="maximumValue">The exclusive maximum value.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> ExclusivelyBetween<T>(
        this ValidatorBuilder<T> builder,
        T minimumValue,
        T maximumValue,
        IComparer<T>? comparer = null,
        Func<T, string>? messageFormatter = null)
        where T : IComparable<T>
    {
        return builder.MustSatisfy(new ValidationRule<T>(
            context =>
            {
                comparer ??= Comparer<T>.Default;
                return comparer.Compare(context.AttemptedValue, minimumValue) > 0 &&
                       comparer.Compare(context.AttemptedValue, maximumValue) < 0;
            },
            messageFormatter ?? (_ => $"Value must be exclusively between {minimumValue} and {maximumValue}.")));
    }

    /// <summary>
    /// Adds a rule that passes validation if the attempted value is not specified value.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="value">The value to compare.</param>
    /// <param name="comparer">The comparer instance used to determine the relational position among values.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> Not<T>(
        this ValidatorBuilder<T> builder,
        T value, 
        IComparer<T>? comparer = null,
        Func<T, string>? messageFormatter = null)
    {
        return builder.MustSatisfy(new ValidationRule<T>(
            context => (comparer ?? Comparer<T>.Default).Compare(context.AttemptedValue, value) != 0,
            messageFormatter ?? (_ => $"Value must not be {value}.")));
    }
    
    /// <summary>
    /// Adds a rule that passes validation if the attempted value matches any of the specified values.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="values">The values to compare.</param>
    /// <param name="equalityComparer">An object that determines equality among values.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> OneOf<T>(
        this ValidatorBuilder<T> builder,
        IEnumerable<T> values,
        IEqualityComparer<T>? equalityComparer = null,
        Func<T, string>? messageFormatter = null)
    {
        equalityComparer ??= EqualityComparer<T>.Default;

        return builder.MustSatisfy(new ValidationRule<T>(
            context => values.Any(value => equalityComparer.Equals(context.AttemptedValue, value)),
            messageFormatter ?? (_ =>
            {
                var valuesString = string.Join(',', values);
                return $"Value must be one of: [{valuesString}]";
            })));
    }
    
    /// <summary>
    /// Adds a rule that passes validation if the attempted value does not match any of the specified values.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="values">The values to compare.</param>
    /// <param name="equalityComparer">An object that determines equality among values.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> NotOneOf<T>(
        this ValidatorBuilder<T> builder,
        IEnumerable<T> values,
        IEqualityComparer<T>? equalityComparer = null,
        Func<T, string>? messageFormatter = null)
    {
        equalityComparer ??= EqualityComparer<T>.Default;

        return builder.MustSatisfy(new ValidationRule<T>(
            context => values.All(value => !equalityComparer.Equals(context.AttemptedValue, value)),
            messageFormatter ?? (_ =>
            {
                var valuesString = string.Join(',', values);
                return $"Value must not be one of: [{valuesString}]";
            })));
    }

    /// <summary>
    /// Adds a rule that passes validation if the attempted value is not the default value for <typeparamref name="T"/>.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> NotDefault<T>(
        this ValidatorBuilder<T> builder,
        Func<T, string>? messageFormatter = null)
    {
        return builder.Not(default!);
    }
}