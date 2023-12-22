using Vertical.CommandLine;
using Vertical.CommandLine.Conversion;

namespace ConsoleApp;

public static class Setup
{
    public static RootCommand BuildRootCommand()
    {
        var pushCommand = new Command("push")
        {
            Bindings =
            {
                new Argument<FileInfo>("root"),
                new Switch("--disable-buffering"),
                new Switch("--interactive"),
                new Option<string?>("--api-key", new[]{"-k"}),
                new Option<string?>("--source", new[]{"-s"}, defaultProvider: () => "/usr/local/.nuget/packages"),
                new Switch("--skip-duplicates"),
                new Option<Point?>("--coordinates", converter: new DelegateConverter<Point?>(str => Point.Parse(str, null)))
            },
            Handler = async (PushArguments model, CancellationToken cancellationToken) =>
            {
                await Task.CompletedTask;
                Console.WriteLine($"Pushing {model.Root} to {model.Source}");
            },
            ModelBinders = { new PushArgumentsBinder() }
        };  

        var rootCommand = new RootCommand
        {
            Commands =
            {
                new Command("nuget")
                {
                    Commands = { pushCommand }
                }
            }
        };

        return rootCommand;
    }
}