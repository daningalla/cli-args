using FluentAssertions;
using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Invocation;

namespace Vertical.CommandLine.Binding.Pipeline;

public class PipelineConverterUsageTests
{
    [Fact]
    public void Pipeline_Uses_Symbol_Converter()
    {
        // arrange
        var command = new RootCommand
        {
            Bindings = { new Argument("arg", converter: new ValueConverter<string?>(_ => "symbol")) },
            Converters =
            {
                new ValueConverter<string?>(_ => "command")
            }
            ,
            Handler = () => { }
        };
        
        // act
        var context = InvocationContextBuilder.Create(command, new[] { "any" });
        var argProvider = context.ArgumentProvider;
        
        // assert
        argProvider.GetValue<string?>("arg").Should().Be("symbol");
    }
    
    [Fact]
    public void Pipeline_Uses_Command_Converter()
    {
        // arrange
        var command = new RootCommand
        {
            Bindings = { new Argument("arg") },
            Converters =
            {
                new ValueConverter<string?>(_ => "command")
            }
            ,
            Handler = () => { }
        };
        
        // act
        var context = InvocationContextBuilder.Create(command, new[] { "any" });
        var argProvider = context.ArgumentProvider;
        
        // assert
        argProvider.GetValue<string?>("arg").Should().Be("command");
    }
    
    [Fact]
    public void Pipeline_Uses_Propagated_Command_Converter()
    {
        // arrange
        var command = new RootCommand
        {
            Converters = { new ValueConverter<string?>(_ => "command") },
            Commands =
            {
                new Command("cmd")
                {
                    Bindings = { new Argument("arg") },
                    Handler = () => { }
                }
            }
        };
        
        // act
        var context = InvocationContextBuilder.Create(command, new[] { "cmd", "any" });
        var argProvider = context.ArgumentProvider;
        
        // assert
        argProvider.GetValue<string?>("arg").Should().Be("command");
    }
}