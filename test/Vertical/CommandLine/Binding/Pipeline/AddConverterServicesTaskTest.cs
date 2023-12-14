using NSubstitute;
using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Invocation;

namespace Vertical.CommandLine.Binding.Pipeline;

public class AddConverterServicesTaskTest
{
    [Fact]
    public void Invoke_Uses_Binding_Converter()
    {
        // arrange
        var (bindingConverter, commandConverter) = (CreateConverter(), CreateConverter());
        var rootCommand = new RootCommand
        {
            Bindings = { new Option<int>("--option", converter: bindingConverter) },
            Converters = { commandConverter },
            Handler = () => { }
        };
        string[] args = { "--option=0" };
        
        // act
        _ = InvocationContextBuilder
            .Create(rootCommand, args)
            .ArgumentProvider
            .GetValue<int>("--option");
        
        // assert
        bindingConverter.Received().Convert(Arg.Any<ConversionContext<int>>());
        commandConverter.DidNotReceive().Convert(Arg.Any<ConversionContext<int>>());
    }
    
    [Fact]
    public void Invoke_Uses_Command_Converter()
    {
        // arrange
        var commandConverter = CreateConverter();
        var rootCommand = new RootCommand
        {
            Bindings = { new Option<int>("--option") },
            Converters = { commandConverter },
            Handler = () => { }
        };
        string[] args = { "--option=0" };
        
        // act
        _ = InvocationContextBuilder
            .Create(rootCommand, args)
            .ArgumentProvider
            .GetValue<int>("--option");
        
        // assert
        commandConverter.Received().Convert(Arg.Any<ConversionContext<int>>());
    }

    [Fact]
    public void Invoke_Uses_Parent_Command_Converter()
    {
        // arrange
        var commandConverter = CreateConverter();
        var rootCommand = new RootCommand
        {
            Commands =
            {
                new Command("command")
                {
                    Bindings = { new Option<int>("--option") },
                    Handler = () => { }
                }
            },
            Converters = { commandConverter },
        };
        string[] args = { "command", "--option=0" };
        
        // act
        _ = InvocationContextBuilder
            .Create(rootCommand, args)
            .ArgumentProvider
            .GetValue<int>("--option");
        
        // assert
        commandConverter.Received().Convert(Arg.Any<ConversionContext<int>>());
    }

    private static IValueConverter<int> CreateConverter()
    {
        var converter = Substitute.For<IValueConverter<int>>();
        converter.ServiceType.Returns(typeof(IValueConverter<int>));
        converter.ValueType.Returns(typeof(int));
        converter.Convert(Arg.Any<ConversionContext<int>>()).Returns(0);
        return converter;
    }
}