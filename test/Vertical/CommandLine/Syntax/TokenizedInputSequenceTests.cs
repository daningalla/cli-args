using FluentAssertions;
using Vertical.CommandLine.Test;

namespace Vertical.CommandLine.Syntax;

public class TokenizedInputSequenceTests
{
    [Fact]
    public void Empty_Returns_Instance_With_No_Tokens()
    {
        // assert
        TokenizedInputSequence.Empty.Length.Should().Be(0);
    }

    [Fact]
    public void Test_Equality()
    {
        // arrange
        const string str = "value";
        var (x, y) = (
            new TokenizedInputSequence(CharacterTokenLexer.GetTokens(str)),
            new TokenizedInputSequence(CharacterTokenLexer.GetTokens(str)));
        
        // assert
        EqualityTest.AssertEqualityOperations(x, y, x == y, x != y);
    }

    [Fact]
    public void Properties_Are_Expected_Values()
    {
        // arrange
        const string str = "value";
        var sequence = new TokenizedInputSequence(CharacterTokenLexer
            .GetTokens(str));
        
        // assert
        sequence.TokenArray.Length.Should().Be(str.Length);
        sequence.Text.Should().Be(str);
        sequence.Span.GetString().Should().Be(str);
        sequence.Length.Should().Be(str.Length);
        for (var c = 0; c < str.Length; c++)
        {
            sequence[c].Value.Should().Be(str[c]);
        }
    }
}