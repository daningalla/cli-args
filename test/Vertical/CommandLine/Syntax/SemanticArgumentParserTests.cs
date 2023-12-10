using FluentAssertions;

namespace Vertical.CommandLine.Syntax;

public class SemanticArgumentParserTests
{
    [Fact]
    public void Parse_Finds_MultiCharacter_Posix_Options()
    {
        // arrange
        var inputs = CreateTokens("-abc");
        
        // act
        var arguments = SemanticArgumentParser.Parse(inputs);

        arguments.Select(arg => arg.Text).Should().Equal("-a", "-b", "-c"); 
    }
    
    [Fact]
    public void Parse_Finds_MultiCharacter_Posix_Options_With_Operand()
    {
        // arrange
        var inputs = CreateTokens("-abc=red");
        
        // act
        var arguments = SemanticArgumentParser.Parse(inputs);

        // assert
        arguments
            .Select(arg => new { arg.Text, arg.SemanticHint })
            .Should()
            .Equal(
                new { Text = "-a", SemanticHint = SemanticHint.KnownSwitch }, 
                new { Text = "-b", SemanticHint = SemanticHint.KnownSwitch }, 
                new { Text = "-c=red", SemanticHint = SemanticHint.None });
    }

    [Fact]
    public void Parse_Assigns_Character_Tokens()
    {
        // arrange
        const string arg = "--key=value";
        var input = CreateTokens(arg);
        
        // act
        var arguments = SemanticArgumentParser.Parse(input);
        
        // assert
        arguments.Single().Tokens.Length.Should().Be(arg.Length);
    }

    [Fact]
    public void Parse_Finds_Known_Switches()
    {
        // arrange
        var input = CreateTokens("-abc", "arg", "--option=value", "-d", "-e");
        
        // act
        var arguments = SemanticArgumentParser.Parse(input);
        
        // assert
        var knownSwitches = arguments
            .Where(arg => arg.IsKnownSwitch)
            .Select(arg => arg.Anatomy.Identifier);
        knownSwitches.Should().Equal("a", "b", "d", "e"); 
    }

    [Fact]
    public void Parse_Finds_Arguments()
    {
        // arrange
        var inputs = CreateTokens("red", "green", "blue");
        
        // act
        var arguments = SemanticArgumentParser.Parse(inputs);
        
        // assert
        arguments
            .Select(arg => new { arg.Text, arg.SemanticHint })
            .Should()
            .Equal(
                new { Text = "red", SemanticHint = SemanticHint.DiscreetArgument },
                new { Text = "green", SemanticHint = SemanticHint.DiscreetArgument },
                new { Text = "blue", SemanticHint = SemanticHint.DiscreetArgument });
    }

    [Fact]
    public void Parse_Finds_Terminated_Arguments()
    {
        // arrange
        var inputs = CreateTokens("red", "--", "green", "blue");
        
        // act
        var arguments = SemanticArgumentParser.Parse(inputs);
        
        // assert
        arguments
            .Select(arg => arg.SemanticHint)
            .Should()
            .Equal(SemanticHint.DiscreetArgument, SemanticHint.Terminated, SemanticHint.Terminated);
    }

    [Fact]
    public void Parse_Identifiers_Speculative_Operand_Arguments()
    {
        // arrange
        var inputs = CreateTokens("-a", "true");
        
        // act
        var arguments = SemanticArgumentParser.Parse(inputs);
        
        // assert
        arguments[1].SemanticHint.Should().Be(SemanticHint.SpeculativeOperand);
    }

    [Theory]
    [InlineData("-a", IdentifierFormat.Posix, "-", "a", "")]
    [InlineData("-a=true", IdentifierFormat.Posix, "-", "a", "true")]
    [InlineData("--color=red", IdentifierFormat.Gnu, "--", "color", "red")]
    [InlineData("/color=red", IdentifierFormat.Microsoft, "/", "color", "red")]
    [InlineData("--color:red", IdentifierFormat.Gnu, "--", "color", "red")]
    [InlineData("/color:red", IdentifierFormat.Microsoft, "/", "color", "red")]
    public void Parse_Identifier_Finds_Correct_Prefix(string arg,
        IdentifierFormat expectedFormat,
        string expectedPrefix,
        string expectedIdentifier,
        string expectedOperand)
    {
        // arrange
        var inputs = CreateTokens(arg);
        
        // act
        var arguments = SemanticArgumentParser.Parse(inputs);
        
        // assert
        var prefix = arguments[0].Anatomy;
        prefix.PrefixFormat.Should().Be(expectedFormat);
        prefix.Identifier.Should().Be(expectedIdentifier);
        prefix.Prefix.Should().Be(expectedPrefix);
        prefix.OperandValueSpan.GetString().Should().Be(expectedOperand);
    }

    private static TokenizedInputSequence[] CreateTokens(params string[] args)
    {
        return args.Select(arg => new TokenizedInputSequence(CharacterTokenLexer.GetTokens(arg))).ToArray();
    }
}