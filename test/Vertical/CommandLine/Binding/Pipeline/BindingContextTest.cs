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
        var bindings = Context.GetValueBindings().ToDictionary(binding => binding.BindingId);
        var binding = (SingleArgumentValueBinding<bool>)bindings[symbol];
        if (expected.HasValue)
            bool.Parse(binding.ArgumentValue!).Should().Be(expected.Value);
        else
            binding.ArgumentValue.Should().BeNull();
    }

    protected void AssertStringBinding(string symbol, string? expected)
    {
        var bindings = Context.GetValueBindings().ToDictionary(binding => binding.BindingId);
        if (expected == null)
        {
            ((SingleArgumentValueBinding<string?>)bindings[symbol]).ArgumentValue.Should().Be(expected);
            return;
        }

        ((SingleArgumentValueBinding<string>)bindings[symbol]).ArgumentValue.Should().Be(expected);
    }

    protected void AssertStringMultiBinding(string symbol, IEnumerable<string> expected)
    {
        var bindings = Context.GetValueBindings().ToDictionary(binding => binding.BindingId);
        var binding = (MultiValueArgumentBinding<string>)bindings[symbol];

        binding.ArgumentValues.Should().Equal(expected);
    }
}