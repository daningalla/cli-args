// See https://aka.ms/new-console-template for more information

using Vertical.CommandLine;
using Vertical.CommandLine.Conversion;

var rootCommand = new RootCommand
{
    Bindings =
    {
        new Switch("--interactive", scope: BindingScope.Descendants),
        new Option<string?>("--api-key", new[]{"-k"}, scope: BindingScope.Descendants),
        new Option<DirectoryInfo?>("--source", new[]{"-s"}, scope: BindingScope.Descendants)
    },
    Commands =
    {
        new Command("push")
        {
            Bindings =
            {
                new Argument<FileInfo>("root", Arity.One),
                new Switch("--no-symbols", new[]{"-n"}),
                new Option<TimeSpan?>("--timeout", new[]{"-t"}, converter: new DelegateConverter<TimeSpan?>(
                    str => TimeSpan.FromSeconds(int.Parse(str))))
            },
            Handler = (
                FileInfo root,
                bool noSymbols,
                TimeSpan? timeout,
                bool interactive,
                string? apiKey,
                DirectoryInfo? source) =>
            {
                Console.WriteLine($"Pushing {root}, noSymbols={noSymbols}, timeout={timeout}, apiKey={apiKey}, source={source}," +
                                  $"interactive={interactive}");   
            }
        },
        new Command("delete")
        {
            Bindings =
            {
                new Argument<string>("package", arity: Arity.One),
                new Argument<string>("version", arity: Arity.One)
            },
            Handler = (
                string package,
                string version,
                bool interactive,
                string? apiKey,
                DirectoryInfo? source
                ) =>
            {
                Console.WriteLine($"Deleting {package}-{version}, interactive={interactive}, apiKey={apiKey}, source={source}");
            }
        }
    }
};

try
{
    rootCommand.Invoke(new[]{"delete", "-z"});
}
catch (CommandLineException exception)
{
    Console.WriteLine(exception.Message);
}


