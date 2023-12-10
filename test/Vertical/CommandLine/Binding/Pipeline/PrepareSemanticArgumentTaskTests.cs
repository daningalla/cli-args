using FluentAssertions;

namespace Vertical.CommandLine.Binding.Pipeline;

public class PrepareSemanticArgumentTaskTests : BindingContextTest
{
    private static readonly string[] Args = {
        "nuget",
        "push",
        "Package.1.0.0.nuspec",
        "--config",
        "Release",
        "-s=$PROFILE/.nuget/.packages"
    };
    
    /// <inheritdoc />
    public PrepareSemanticArgumentTaskTests() : base(Setup())
    {
    }

    [Fact]
    public void Invoke_Adds_Semantic_Arguments()
    {
        // assert
        var results = Context
            .StagedSemanticArguments
            .Select(arg => arg.Text)
            .OrderBy(arg => arg);

        results.Should().Equal(Args.Skip(2).OrderBy(arg => arg));
    }

    private static IBindingContext Setup()
    {
        return BindingPipeline.CreateContext(new RootCommand
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
                                new Argument("project"),
                                new Option("--config"),
                                new Option("-s")
                            },
                            Handler = () => { }
                        }
                    }
                }
            }
        }, Args, CancellationToken.None);
    }
}