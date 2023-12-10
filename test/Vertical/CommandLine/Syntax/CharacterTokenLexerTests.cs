using FluentAssertions;

namespace Vertical.CommandLine.Syntax;

public class CharacterTokenLexerTests
{
    [Fact]
    public void GetTokens_Returns_Empty()
    {
        CharacterTokenLexer.GetTokens(string.Empty).Should().HaveCount(0);
    }

    [Fact]
    public void GetTokens_Returns_Literals()
    {
        const string value = "red";
        var tokens = CharacterTokenLexer.GetTokens(value);
        var expected = Enumerable
            .Range(0, value.Length)
            .Select(i => new CharacterToken(value, i, CharacterType.LiteralValueToken));
        tokens.Should().Equal(expected);
    }

    [Fact]
    public void GetTokens_Returns_Single_Literal()
    {
        var tokens = CharacterTokenLexer.GetTokens("-");
        tokens.Should().Equal(new CharacterToken("-", 0, CharacterType.LiteralValueToken));
    }

    [Fact]
    public void GetTokens_Returns_Short_Identifier()
    {
        const string arg = "-a";
        var tokens = CharacterTokenLexer.GetTokens(arg);
        tokens.Should().Equal(
            new CharacterToken(arg, 0, CharacterType.PrefixToken),
            new CharacterToken(arg, 1, CharacterType.IdentifierToken));
    }

    [Fact]
    public void GetTokens_Returns_Short_MultiCharacter_Identifier()
    {
        const string arg = "-abc";
        var tokens = CharacterTokenLexer.GetTokens(arg);
        var id = 0;
        var expected = new[]
        {
            new CharacterToken(arg, id++, CharacterType.PrefixToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id, CharacterType.IdentifierToken)
        };
        tokens.Should().Equal(expected);
    }

    [Fact]
    public void GetTokens_Returns_Long_Identifier()
    {
        const string arg = "--long-opt";
        var tokens = CharacterTokenLexer.GetTokens(arg);
        var id = 0;
        var expected = new[]
        {
            new CharacterToken(arg, id++, CharacterType.PrefixToken),
            new CharacterToken(arg, id++, CharacterType.PrefixToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id, CharacterType.IdentifierToken)
        };
        tokens.Should().Equal(expected);
    }

    [Fact]
    public void GetTokens_Returns_Short_Identifier_With_Operand()
    {
        const string arg = "-a=true";
        var tokens = CharacterTokenLexer.GetTokens(arg);
        var id = 0;
        var expected = new[]
        {
            new CharacterToken(arg, id++, CharacterType.PrefixToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.OperandAssignmentToken),
            new CharacterToken(arg, id++, CharacterType.LiteralValueToken),
            new CharacterToken(arg, id++, CharacterType.LiteralValueToken),
            new CharacterToken(arg, id++, CharacterType.LiteralValueToken),
            new CharacterToken(arg, id, CharacterType.LiteralValueToken)
        };
        tokens.Should().Equal(expected);
    }

    [Fact]
    public void GetTokens_Returns_Short_MultiCharacterIdentifier_With_Operand()
    {
        const string arg = "-abc=true";
        var tokens = CharacterTokenLexer.GetTokens(arg);
        var id = 0;
        var expected = new[]
        {
            new CharacterToken(arg, id++, CharacterType.PrefixToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.OperandAssignmentToken),
            new CharacterToken(arg, id++, CharacterType.LiteralValueToken),
            new CharacterToken(arg, id++, CharacterType.LiteralValueToken),
            new CharacterToken(arg, id++, CharacterType.LiteralValueToken),
            new CharacterToken(arg, id, CharacterType.LiteralValueToken)
        };
        tokens.Should().Equal(expected);
    }

    [Fact]
    public void GetTokens_Returns_Long_Identifier_With_Operand()
    {
        const string arg = "--long-opt=true";
        var tokens = CharacterTokenLexer.GetTokens(arg);
        var id = 0;
        var expected = new[]
        {
            new CharacterToken(arg, id++, CharacterType.PrefixToken),
            new CharacterToken(arg, id++, CharacterType.PrefixToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.IdentifierToken),
            new CharacterToken(arg, id++, CharacterType.OperandAssignmentToken),
            new CharacterToken(arg, id++, CharacterType.LiteralValueToken),
            new CharacterToken(arg, id++, CharacterType.LiteralValueToken),
            new CharacterToken(arg, id++, CharacterType.LiteralValueToken),
            new CharacterToken(arg, id, CharacterType.LiteralValueToken)
        };
        tokens.Should().Equal(expected);
    }

    [Fact]
    public void GetTokens_Returns_Unknown_Identifiers()
    {
        const string arg = "--/id";
        var tokens = CharacterTokenLexer.GetTokens(arg);
        var id = 0;
        var expected = new[]
        {
            new CharacterToken(arg, id++, CharacterType.PrefixToken),
            new CharacterToken(arg, id++, CharacterType.PrefixToken),
            new CharacterToken(arg, id++, CharacterType.InvalidIdentifierToken),
            new CharacterToken(arg, id++, CharacterType.InvalidIdentifierToken),
            new CharacterToken(arg, id, CharacterType.InvalidIdentifierToken)
        };
        tokens.Should().Equal(expected);
    }
}