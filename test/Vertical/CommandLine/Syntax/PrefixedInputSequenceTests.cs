using FluentAssertions;

namespace Vertical.CommandLine.Syntax;

public class PrefixedInputSequenceTests
{
    [Fact]
    public void Create_Returns_Instance_With_Operand()
    {
        // arrange
        var tokens = CharacterTokenLexer.GetTokens("--long-option=value");
        var input = new TokenizedInputSequence(tokens);
        
        // act
        var instance = SemanticAnatomy.Create(input);
        
        // assert
        instance.Prefix.Should().Be("--");
        instance.PrefixedIdentifier.Should().Be("--long-option");
        instance.Identifier.Should().Be("long-option");
        instance.OperandAssignmentOperator.Should().Be("=");
        instance.OperandValueSpan.GetString().Should().Be("value");
    }
}