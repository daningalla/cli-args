using FluentAssertions;
using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Test;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine.Binding.Pipeline;

public class AddServicesInPathTaskTests
{
    [Fact]
    public void Invoke_Adds_Services()
    {
        // arrange
        var command = new RootCommand
        {
            Converters = { Substitutes.CreateBindingService<IValueConverter<bool>>() },
            Validators = { Substitutes.CreateBindingService<IValidator<char>>() },
            Commands =
            {
                new Command("child")
                {
                    Converters = { Substitutes.CreateBindingService<IValueConverter<short>>() },
                    Validators = { Substitutes.CreateBindingService<IValidator<int>>() },
                    Handler = () => { }
                }
            }
        };

        // act
        var context = BindingPipeline.CreateContext(command, new[] { "child" }, CancellationToken.None);
        
        // assert
        context.Services.GetRequiredService<IValueConverter<bool>>().Should().NotBeNull();
        context.Services.GetRequiredService<IValidator<char>>().Should().NotBeNull();
        context.Services.GetRequiredService<IValueConverter<short>>().Should().NotBeNull();
        context.Services.GetRequiredService<IValidator<int>>().Should().NotBeNull();
    }
}