// See https://aka.ms/new-console-template for more information

using ConsoleApp;
using Vertical.CommandLine;

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
        new Option<TimeSpan?>("--timeout")
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

try
{
    args = new[]
    {
        "nuget",
        "push",
        "/usr/src/vertical.commandline.csproj",
        "--api-key=annsiju9889792hkjhdb82730i2hj9j292oth939",
        "--skip-duplicates"
    };
    await rootCommand.InvokeAsync(args, CancellationToken.None);
}
catch (CommandLineException exception)
{
    Console.WriteLine(exception.Message);
}


