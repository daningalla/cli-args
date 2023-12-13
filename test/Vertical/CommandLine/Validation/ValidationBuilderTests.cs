using System.Text.RegularExpressions;
using FluentAssertions;

namespace Vertical.CommandLine.Validation;

public class ValidationBuilderTests
{
    [Fact]
    public void MinimumLength_Creates_Validator_Returns_Expected() => AssertValidatorBehavior<string?>(
        b => b.MinimumLength(5), "hello", "bye");
    
    [Fact]
    public void MaximumLength_Creates_Validator_Returns_Expected() => AssertValidatorBehavior<string?>(
        b => b.MaximumLength(5), "hello", "goodbye");
    
    [Fact]
    public void Matches_Pattern_Creates_Validator_Returns_Expected() => AssertValidatorBehavior<string?>(
        b => b.Matches(@"\d{5}"), "80210", "abcde");
    
    [Fact]
    public void Matches_Regex_Creates_Validator_Returns_Expected() => AssertValidatorBehavior<string?>(
        b => b.Matches(new Regex(@"\d{5}")), "80210", "abcde");

    [Fact]
    public void Contains_Creates_Validator_Returns_Expected() => AssertValidatorBehavior<string>(
        b => b.Contains("green"), "red-green-blue", "red-blue");
    
    [Fact]
    public void StartsWith_Creates_Validator_Returns_Expected() => AssertValidatorBehavior<string>(
        b => b.StartsWith("abc"), "abcdef", "def");
    
    [Fact]
    public void EndsWith_Creates_Validator_Returns_Expected() => AssertValidatorBehavior<string>(
        b => b.EndsWith("def"), "abcdef", "abc");
    
    [Fact]
    public void LessThan_Creates_Validator_Returns_Expected() => AssertValidatorBehavior(
        b => b.LessThan(1), 0, 1);

    [Fact]
    public void LessThanOrEquals_Creates_Validator_Returns_Expected() => AssertValidatorBehavior(
        b => b.LessThanOrEquals(1), new[] { 0, 1 }, 2);
    
    [Fact]
    public void GreaterThan_Creates_Validator_Returns_Expected() => AssertValidatorBehavior(
        b => b.GreaterThan(1), 2, 0);
    
    [Fact]
    public void GreaterThanOrEquals_Creates_Validator_Returns_Expected() => AssertValidatorBehavior(
        b => b.GreaterThanOrEquals(1), new[]{ 1, 2 }, 0);
    
    [Fact]
    public void Not_Creates_Validator_Returns_Expected() => AssertValidatorBehavior(
        b => b.Not(1), 0, 1);
    
    [Fact]
    public void OneOf_Creates_Validator_Returns_Expected() => AssertValidatorBehavior(
        b => b.OneOf(new[]{1,2,3}), 2, 4);
    
    [Fact]
    public void NotOneOf_Creates_Validator_Returns_Expected() => AssertValidatorBehavior(
        b => b.NotOneOf(new[]{1,2,3}), 4, 2);

    private static void AssertValidatorBehavior<T>(
        Action<ValidatorBuilder<T>> configure,
        T validValue,
        T invalidValue)
    {
        // arrange
        var builder = new ValidatorBuilder<T>();
        configure(builder);
        var validator = builder.Build();
        
        // assert
        AssertContextState(validator, validValue, true);
        AssertContextState(validator, invalidValue, false);
    }
    
    private static void AssertValidatorBehavior<T>(
        Action<ValidatorBuilder<T>> configure,
        T[] validValues,
        T invalidValue)
    {
        // arrange
        var builder = new ValidatorBuilder<T>();
        configure(builder);
        var validator = builder.Build();
        
        // assert
        foreach (var validValue in validValues)
        {
            AssertContextState(validator, validValue, true);
        }
        AssertContextState(validator, invalidValue, false);
    }

    private static void AssertContextState<T>(IValidator<T> validator, T value, bool valid)
    {
        // arrange
        var context = new ValidationContext<T>(new Option<T>("--option"), value);
        
        // act
        validator.Validate(context);
        
        //assert
        context.IsValid.Should().Be(valid);
        context.Failures.Should().HaveCount(valid ? 0 : 1);
    }
}