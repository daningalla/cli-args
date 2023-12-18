using FluentAssertions;

namespace Vertical.CommandLine.Utilities;

public class ConfigurationValidatorTests
{
    [Fact]
    public void Validate_Adds_Command_Identifier_Exception()
    {
        // arrange
        var command = new RootCommand
        {
            Commands =
            {
                new Command("%invalid")
            },
            Handler = () => { }
        };
        
        // assert
        AssertHasExceptions(command);
    }

    [Theory]
    [InlineData("option")]
    public void Validate_Adds_Option_Symbol_Identifier_Exception(string symbolId)
    {
        // arrange
        var command = new RootCommand
        {
            Bindings = { new Option<string>(symbolId) },
            Handler = () => { }
        };
        
        // assert
        AssertHasExceptions(command);
    }

    [Fact]
    public void Validate_Adds_Command_Unique_Identifiers_Exception()
    {
        // arrange
        var command = new RootCommand
        {
            Commands =
            {
                new Command("red"){ Handler = () => { }  },
                new Command("green", new[] { "red" }){ Handler = () => { }  },
                new Command("blue", new[] { "green" }){ Handler = () => { }  }
            }
        };
        
        // assert
        AssertHasExceptions(command);
    }

    [Fact]
    public void Validate_Adds_Command_Missing_Handler_Exception()
    {
        // arrange
        var command = new RootCommand
        {
            Bindings = { new Argument<string>("project", scope: BindingScope.Self) }
        };
        
        // assert
        AssertHasExceptions(command);
    }

    [Fact]
    public void Validate_Adds_Argument_Arity_Exception()
    {
        // arrange
        var command = new RootCommand
        {
            Bindings =
            {
                new Argument<string>("colors"),
                new Argument<string>("path", Arity.One)
            },
            Handler = () => { }
        };
        
        // assert
        AssertHasExceptions(command);
    }

    [Fact]
    public void Validate_Adds_Handler_Return_Type_Exception()
    {
        // arrange
        var command = new RootCommand
        {
            Handler = () => 0,
            Commands =
            {
                new Command("is-bool")
                {
                    Handler = () => true
                }
            }
        };
        
        // assert
        AssertHasExceptions(command);
    }

    [Fact]
    public void Validate_Adds_Parameter_Binding_Name_Exception()
    {
        // arrange
        var command = new RootCommand
        {
            Bindings = { new Switch("--no-restore") },
            Handler = (bool restore) => 0
        };
        
        // assert
        AssertHasExceptions(command);
    }

    [Fact]
    public void Validate_Adds_Parameter_Binding_Type_Exception()
    {
        // arrange
        var command = new RootCommand
        {
            Bindings = { new Switch("--no-restore") },
            Handler = (Guid noRestore) => 0
        };
        
        // assert
        AssertHasExceptions(command);
    }

    [Fact]
    public void Validate_Adds_Missing_Converter_Type_Exception()
    {
        // arrange
        var command = new RootCommand
        {
            Commands =
            {
                new Command("nuget")
                {
                    Commands =
                    {
                        new Command("push")
                        {
                            Bindings =
                            {
                                new Option<IntPtr>("--int-ptr"),
                                new Option<Range>("--range")
                            },
                            Handler = () => { }
                        },
                        new Command("delete")
                        {
                            Bindings = { new Option<Range>("--range") },
                            Handler = () => { }
                        }
                    }
                }
            }
        };
        
        // assert
        AssertHasExceptions(command);
    }

    private static void AssertHasExceptions(RootCommand command)
    {
        // act
        var exceptions = ConfigurationValidator.Validate(command);
        
        // assert
        exceptions.Should().NotBeEmpty();
    }
}