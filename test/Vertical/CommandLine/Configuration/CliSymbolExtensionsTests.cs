using FluentAssertions;

namespace Vertical.CommandLine.Configuration;

public class CliSymbolExtensionsTests
{
    [Theory]
    [InlineData("id", new string[]{}, "id", true)]
    [InlineData("id", new[]{"alias"}, "alias", true)]
    [InlineData("id", new[]{"alias-1", "alias-2"}, "alias-1", true)]
    [InlineData("id", new[]{"alias-1", "alias-2"}, "alias-0", false)]
    public void IsMatchToAnyType_Returns_Expected(string id, string[] aliases, string parameter, bool expected)
    {
        // arrange
        var symbol = new Option(id, aliases);
        
        // act/assert
        symbol.IsMatchToAnyIdentifier(parameter).Should().Be(expected);
    }

    [Theory]
    [InlineData(0, 1, 0, false)]
    [InlineData(0, null, 0, false)]
    [InlineData(1, null, 1, false)]
    [InlineData(1, 2, 1, false)]
    [InlineData(1, 2, 2, false)]
    [InlineData(1, null, 0, true)]
    [InlineData(1, 1, 0, true)]
    [InlineData(1, 2, 0, true)]
    [InlineData(1, 1, 2, true)]
    public void Validate_Arity_Returns_Expected(int minCount, int? maxCount, int argCount, bool throws)
    {
        // arrange/act
        var symbol = new Argument("arg", new Arity(minCount, maxCount));
        
        // assert
        if (throws)
        {
            symbol
                .Invoking(x => x.ValidateArity(argCount, Enumerable.Empty<string>))
                .Should()
                .Throw<Exception>();
            return;
        }
        
        symbol
            .Invoking(x => x.ValidateArity(argCount, Enumerable.Empty<string>))
            .Should().NotThrow();
    }
}