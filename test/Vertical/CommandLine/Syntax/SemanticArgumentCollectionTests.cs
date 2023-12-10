using FluentAssertions;

namespace Vertical.CommandLine.Syntax;

public class SemanticArgumentCollectionTests
{
    private static readonly string[] Args =
    {
        "push",
        "--color=red",
        "--size", "square"
    };

    private readonly SemanticArgumentCollection _instance = new(
        SemanticArgumentParser.Parse(Args
            .Select(arg => new TokenizedInputSequence(CharacterTokenLexer.GetTokens(arg)))
            .ToArray()));

    [Fact] 
    public void IsEmpty_Returns_False() => _instance.IsEmpty.Should().BeFalse();

    [Fact]
    public void TryPeekArgumentValue_Returns_True_With_First_Argument()
    {
        // arrange/act
        var result = _instance.TryPeekValueArgument(0, out var argument);
        
        // assert
        result.Should().BeTrue();
        argument!.Text.Should().Be("push");
    }

    [Fact]
    public void RemoveArgument_Returns_Instance()
    {
        // arrange/act
        _instance.RemoveArgument(_instance.First());
        
        // assert
        _instance.TryPeekValueArgument(0, out _).Should().BeFalse();
    }

    [Fact]
    public void RemoveOptionArguments_Returns_Tuple_With_Speculative_Operand()
    {
        // arrange/act
        var pairs = _instance.RemoveOptionArguments(new Option("--size"));
        
        // assert
        pairs.Should().HaveCount(1);
        pairs.Single().MatchedArgument.Text.Should().Be("--size");
        pairs.Single().SpeculativeOperand!.Text.Should().Be("square");
    }
}