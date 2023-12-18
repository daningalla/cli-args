using FluentAssertions;
using Vertical.CommandLine.Test;

namespace Vertical.CommandLine.Binding.Pipeline;

public class AddBindingSymbolsTaskTests
{
    [Theory, MemberData(nameof(Theories))]
    public void Invoke_Adds_Correct_Symbols(RootCommand command, string[] args, string[] expectedOptions)
    {
        // arrange/act
        var context = BindingPipeline.CreateContext(command, args, CancellationToken.None);
        
        // assert
        context
            .StagedBindingSymbols
            .Select(symbol => symbol.Id)
            .Should()
            .Equal(expectedOptions);
    }

    public static IEnumerable<object[]> Theories => TheoryBuilder.Build(t => t
        .AddCase(
            new RootCommand
            {
                Bindings = { new Option<string>("--1") },
                Handler = () => { }
            },
            Array.Empty<string>(),
            new[] { "--1" })

        // No child symbols because command is not invoked
        .AddCase(
            new RootCommand
            {
                Bindings = { new Option<string>("--1"), },
                Handler = () => { },
                Commands =
                {
                    new Command("child")
                    {
                        Bindings = { new Option<string>("--2") },
                        Handler = () => { }
                    }
                }
            },
            Array.Empty<string>(),
            new[] { "--1" })

        // Child symbols only, root bindings scoped "Self"
        .AddCase(
            new RootCommand
            {
                Bindings = { new Option<string>("--1"), },
                Commands =
                {
                    new Command("child")
                    {
                        Bindings = { new Option<string>("--2") },
                        Handler = () => { }
                    }
                },
                Handler = () => { }
            },
            new[] { "child" },
            new[] { "--2" })
        
        // Child + root symbols, child bindings scoped "SelfAndDescendents"
        .AddCase(
            new RootCommand
            {
                Bindings = { new Option<string>("--1", scope: BindingScope.Descendents) },
                Handler = () => { },
                Commands =
                {
                    new Command("child")
                    {
                        Bindings = { new Option<string>("--2", scope: BindingScope.SelfAndDescendents) },
                        Handler = () => { }
                    }
                }
            },
            new[] { "child" },
            new[] { "--1", "--2" })
    
        // Child can overwrite symbols with the same id
        .AddCase(
            new RootCommand
            {
                Bindings = { new Option<string>("--1", scope: BindingScope.Descendents), },
                Handler = () => { },
                Commands =
                {
                    new Command("child")
                    {
                        Bindings = { new Option<string>("--1") },
                        Handler = () => { }
                    }
                }
            },
            new[] { "child" },
            new[] { "--1" })
    );
}