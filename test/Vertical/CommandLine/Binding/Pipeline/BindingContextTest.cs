using FluentAssertions;

namespace Vertical.CommandLine.Binding.Pipeline;

public abstract class BindingContextTest
{
    protected readonly IBindingContext Context;

    protected BindingContextTest(IBindingContext context)
    {
        Context = context;
    }
    
    protected void AssertBooleanBinding(string symbol, bool? expected)
    {
        // arrange
        var bindings = Context.GetValueBindings().ToDictionary(binding => binding.BindingId);
        var binding = (ArgumentValueBinding<bool>)bindings[symbol];
        
        // act
        var value = binding.ArgumentValues.FirstOrDefault();
        
        if (expected.HasValue)
            bool.Parse(value!).Should().Be(expected.Value);
        else
            value.Should().BeNull();
    }

    protected void AssertStringBinding(string symbol, string? expected)
    {
        // arrange
        var bindings = Context.GetValueBindings().ToDictionary(binding => binding.BindingId);
        
        // act
        var value = bindings[symbol].ArgumentValues.FirstOrDefault();

        // assert
        value.Should().Be(expected);
    }

    protected void AssertStringMultiBinding(string symbol, IEnumerable<string> expected)
    {
        var bindings = Context.GetValueBindings().ToDictionary(binding => binding.BindingId);
        var binding = (ArgumentValueBinding<string>)bindings[symbol];

        binding.ArgumentValues.Should().Equal(expected);
    }
}