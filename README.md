# vertical-cli

A command-line argument parser.

> Note
>
> This library was made for internal use and as an experiment/learning path for source generator development. Updates, enhancements, and patches are not guaranteed. Use at your own risk. This library was motivated by and shares a lot of semantics with System.CommandLine.


## Overview

This library parses command line arguments. Top level features include:

- Parses POSIX, GNU and Microsoft style option syntax.
- Provides strongly-typed, mapped arguments to application defined delegates.
- No runtime reflection/AOT trim friendly and optimized using source generation.
- Provides robust configuration validation for unit testing.
- Supports hierarchical routing using sub-command definitions.
- Provides interfaces for custom value conversion, validation, and complex type binding.

## Quick setup

```
$ dotnet add package vertical-cli --pre
```

```csharp
using Vertical.CommandLine;

enum CompressionType { None, GZip }

// Program.cs
var rootCommand = new RootCommand
{
    // Define options and arguments
    Bindings = 
    {
        new Argument<FileInfo>("source", arity: Arity.One),
        new Argument<FileInfo>("dest", arity: Arity.One),
        new Option<CompressionType>("--compression")
    },
    
    // Define a delegate to receive the arguments
    Handler = async (
        FileInfo source, 
        FileInfo dest, 
        CompressionType compression, 
        CancellationToken cancelToken) => 
        {
            using var sourceStream = File.OpenRead(source);
            using var destStream = CompressionType.GZip == compression
                ? new GZipStream(File.OpenWrite(dest), CompressionMode.Compress)
                : File.OpenWrite(dest);

            await sourceStream.CopyToAsync(destStream);
            Console.WriteLine($"File {source} copied.");            
        }
};

// Invoke the program
await rootCommand.InvokeAsync(args, CancellationToken.None);
```

```
$ dotnet run -- ~/myfile.txt ~/myfile.gzip --compression gzip
```

## Configuration

### Commands

_Commands_ represent an action your application can perform. It consists of strongly-typed argument and option definitions (called _bindings_), and a delegate that receives the parsed argument values. The delegate then uses the values to perform the function of the application. An application must define a _root command_. After a `RootCommand` instance is configured, arguments can be parsed and routed to the handler with `Invoke`/`InvokeAsync` extension methods.

> Note
>
> The `Invoke` and `InvokeAsync` extension methods are source generated. If they are not available, rebuild your project.

Commands can have sub-commands that perform different actions. Consider `dotnet nuget push` and `dotnet nuget delete` tool commands. Whereas `dotnet` is the program name and would be defined as the root command, `nuget` is a sub-command defined within the root command, and `push` and `delete` are sub-commands defined within the `nuget` command. The above could be minimally configured as follows:

```csharp
var nugetCommand = new Command("nuget")
{
    Commands = 
    {
        new Command("push"){ /* configuration */ },
        new Command("delete"){ /* configuration */ }
    }
}
 
var rootCommand = new RootCommand
{
    Commands = { nugetCommand }
}
```

### Options

_Options_ are program argument values that are paired with a prefixed identifier. Option identifiers provided on the CLI can take the following forms:

- POSIX single character format, e.g. `-a`.
- POSIX multi-character format, e.g. `-abc`
- GNU long option format, e.g. `--option` or `--long-option`
- Microsoft style options, e.g. `/p` or `/path`

On the CLI, operand values can be paired to an identifier in the following ways:

- Operand is the next immediate argument after the identifier and is separated by a space, e.g. `-p /var/lib`.
- The operand is placed after the identifier with no space and separated by a colon `:` or equal characeter `=`, e.g. `-p=/var/lib` or `-p:/var/lib`.

A _Switch_ is a specialized form of option that can only have a boolean operand value which does not have to be explicitly provided (`true` is inferred). The following are all equivelent:

- `-a`
- `-a=true`
- `-a:true`

Single character identifier switch can be combined on the CLI for brevity. Additionally, single character switches can be combined with an option. For example:

- `-abc` is equivalent to specifying switches `-a`, `-b`, and `-c`
- `-abc=value` or `-abc value` is equivalent to switches `-a`, `-b` and option `-c=value`

Options are configured by adding a _binding_ to a command's `Bindings` collection. At a minimum, you must specify the value type and a prefixed identifier. Options and switches are configured by creating a new `Option<T>` or `Switch` instance with a unique prefixed identifier that the CLI parser will use to match to arguments.

### Arguments

_Arguments_ are values that are not paired with a prefixed identifier on the CLI. Arguments are defined in the `Bindings` collection along side options and switches. The parser first matches options and switches and consumes any operands that are paired, leaving the remaining CLI arguments to be mapped to configured `Argument<T>` bindings. Argument identifiers do not start with prefix characters.

### Arity

Command bindings can define an _arity_, which represents the required and/or allowable count of values that the option, argument, or switch can bind to. The `Arity` structure defines these expectations, and takes two values in its constructor:

- `minCount`: The required minimum number of values.
- `maxCount`: The maximum allowable number of values. If this value is `null`, then the maximum count is unconstrained.

The `Arity` structure also defines several static properties that serve as abbreviations to common scenarios:

- `ZeroOrOne` - use optional, but cannot bind to more than one value
- `ZeroOrMany` - use optional, but can bind to many values
- `One` - exactly one value is required (not optional)
- `OneOrMany` - at least one value is required, but can bind to multiple values

An exmaple of CLI input to a multi-valued option: `program --path /bin --path /dev`, or to a multi-valued argument: `blend #ff0000 #00ff00 #0000ff`.

When you define a binding that has multi-valued arity, the matching parameter type should be some sort of collection. For example:

```csharp
var rootCommand = new RootCommand
{
    Bindings = { new Argument<string>("paths") },
    Handler = (string[] paths) => { /* ... */ }
}
```

The library has binding support for:
- Concrete collections: `Array`, `List<T>`, `LinkedList<T>`, `HashSet<T>`, `SortedSet<T>`, `Stack<T>`, and `Queue<T>`
- Any type that is `ICollection<T>`, `IReadOnlyCollection<T>`, `IEnumerable<T>`, `IList<T>`, `IReadOnlyList<T>`, `ISet<T>`, `IReadOnlySet<T>`

### Aliases

Options and switches can define _aliases_, which are other prefixed identifiers the binding can be referred to. For example, we may want a path option to have two names in the CLI.

```csharp
var pathOption = new Option<string>("--path", aliases: new[]{"-p"});
```

All primary identifiers, as well as aliases, must be unique within the `Bindings` collection of a command.

> Note
>
> The library will bind parameters and model properties only to primary identifiers, not to aliases.

### Handlers

Handlers are delegates that receive the parsed argument values as strongly-typed parameters. Note the following when defining each delegate:

- Every command must define a handler, except the case in which a command does not perform an action but delegates program flow to one of its sub-commands.
- Every handler must return the same type. For instance, a root command handler cannot return an `int` while a sub-command handler returns `void`. Furthermore, all handlers must follow the same synchronous or asynchronous result type.
- Parameter names are matched to or inferred from the _primary_ identifier of the option, switch, or argument binding (not to aliases) . For instance, `--path` will be automatically matched to parameter `path`, `--output-path` matched to parameter `outputPath`, etc. Explicit bindings can be specified (see advanced configuration).

## Advanced configuration

### Default values

When CLI input omits an option or argument that has an optional arity, the parsed value will be the default type of the handler parameter (equivalent to `default(T)`). Applications can define a custom default value by specifying a function in the constructor of the `Argument<T>` or `Option<T>` types. The function will be called when the default value is required.

```csharp
var option = new Option<DateTime?>("--date", defaultProvider: () => DateTime.Now);
```

### Explicit binding names

If the application needs to bind to a parameter or property where the name cannot be matched by default, an explicit binding can be made using `[BindingAttribute]` as shown in the following example:

```csharp
var rootCommand = new RootCommand
{
    Bindings = new Option<string>("--path"),

    // 'paths' won't bind to '--path', so use the explicit binding attribute
    Handler = ([Binding("--path")] IEnumerable<string> paths) => { /*...*/ }
};
```

### Binding different types

The library will automatically convert from `string` CLI arguments to the following types:

- All numeric types defined in the `System` namespace including `Half` and `Int128`, and their `Nullable<T>` counterparts.
- Character types `char` and `string`.
- Temporal types `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`, and `TimeSpan`.
- `Guid`
- `FileInfo`, `DirectoryInfo`, and `Uri`.
- `Enums`

If you want to have the library bind to a type that isn't supported, you must define a value converter. The following example shows two ways to accomplish this.

```csharp
// Type not convertible out-of-box
public readonly struct Point(double X, double Y) : IParsable<Point>
{
    public static Point Parse(string s, IFormatProvider? provider){ /* ... */ }
    public static bool TryParse(string? s, IFormatProvider? provider, out Point result) { /* ... */ }
}

// Define a converter inline.
var option = new Option<Point>("--coordinates", converter: new DelegateConverter<Point>(Point.Parse));

// Or use an application-defined converter type
public class PointConverter : ValueConverter<Point>
{
    public override Point Convert(ConversionContext<T> context) =>
        Point.Parse(context.Value, provider: null);
}

var option = new Option<Point>("--coordinates", converter: new PointConverter());

```

Value converters can also be introduced in a command's `Converters` collection. Doing so makes the converter available to all bindings in the command and sub-command paths.

```csharp
var command = new Command("build")
{
    Converters = { new ValueConverter<Point>(Point.Parse) }
}
```

### Validation

After argument values are converted to their binding type, they can be additionally validated. The following example demonstrates how to validate a phone number.

```csharp
// Define validation inline.
var option = new Argument<string>("phone", validator: Validator.Build<string>(value =>
    value.Matches(@"\d{3}-\d{3}-\d{4}")));

// Define a validator implementation
public class PointValidator : Validator<Point>
{
    public override ValidationResult Validate(Point value)
    {
        return value.X >= 0 && value.Y >= 0
            ? ValidationResult.Success
            : ValidationResult.Fail("Coordinates cannot be negative.");
    }
}    
```

Like converters, validators can also be introduced in a command's `Validators` collection. Doing so makes the validator available to all bindings in the command and sub-command paths that match the type.

### Binding scope

By default, option and argument bindings are only available to the command in which they are defined. There may be cases where the same option or argument will be defined across all commands, or particular commands in a sub-path. The library provides the notion of _scopes_ to help you not repeat yourself. The three scopes in the `BindingScope` enum are:

- `Self` - The default, the binding will only apply to the command in which it is defined
- `SelfAndDescendants` - The binding will apply to the command in which it is defined and inherited by any sub-commands.
- `Descendants` - The binding will be inherited by sub-commands.

```csharp
var rootCommand = new RootCommand
{
    // Push this down to commands
    Bindings = { new Argument<FileInfo>("path", scope: BindingScope.Descendants) },
    Commands = 
    {
        new Command("delete")
        {
            Bindings = { new Switch("--confirm") },
            Handler = (FileInfo path, bool confirm) => { /* ... */ }
        },
        new Command("print")
        {
            Handler = (FileInfo path) => { /* ... */ }
        }
    }
}
```

### Model binding

An application can aggregate argument values into an application model by using an implementation of `ModelBinder<T>`. The following example demonstrates this:

```csharp
// Models
public enum CompressionType { None, GZip }
public record CompressParameters(FileInfo Source, FileInfo Dest, CompressionType Compression);

// Binder implementation
public class CompressParametersBinder : ModelBinder<CompressParameters>
{
    protected override CompressParameters BindInstance(IMappedArgumentProvider arguments)
    {
        return new CompressParameters(
            Source: arguments.GetValue<FileInfo>("source"),
            Dest: arguments.GetValue<FileInfo>("dest"),
            Compression: arguments.GetValue<CompressionType>("--compression")
        );
    }
}

// Root command setup
var rootCommand = new RootCommand
{
    Bindings =
    {
        new Argument<FileInfo>("source"),
        new Argument<FileInfo>("dest"),
        new Option<CompressionType>("--compression")
    },

    // Register binder
    ModelBinders = { new CompressParametersBinder() },

    // Define handler
    Handler = async (CompressParameters parameters, CancellationToken cancel) => 
    {
        await using var sourceStream = File.OpenRead(parameters.Source);
        await using var destStream = parameters.Compression == CompressionType.GZip
            ? new GZipStream(File.OpenWrite(parameters.Dest), CompressionMode.Compress)
            : File.OpenWrite(parameters.Dest);

        await sourceStream.CopyToAsync(destStream, cancel);            
    }
}
```

### Automatic binder generation
 
The library has a source generator that will automatically build binders for you by simply using the `GeneratedBinding` attribute. We'll modify the above example to leverage the source generator.

```csharp
// Models
public enum CompressionType { None, GZip }

// Note usage of this attribute is not required on the model itself, but should be used if the model type is under active development (triggers the source generator). 
[GeneratedBinding]
public record CompressParameters(FileInfo Source, FileInfo Dest, CompressionType Compression);

// Binder implementation
[GeneratedBinding<CompressParameters>]
public partial class CompressParametersBinder : ModelBinder<CompressParameters>
{
}

// Root command setup
var rootCommand = new RootCommand
{
    /* setup */
}
```

## Handling exceptions

The library will throw exceptions during validation in `DEBUG` mode if the root command configuration is invalid. These are typically the `InvalidOperationException` and `ArgumentException` types.

When a user of your application makes an error, the library will throw a `CommandLineException`. The application can catch the exception and display the message to the user. Examples of invalid CLI input caught by the library are as follows:

- An option or argument's arity was violated (`CommandLineArityException`).
- An argument was not provided for an option (`CommandLineOptionException`).
- A sub-command was not matched when the root command does not have a handler (`CommandLineInvocationException`).
- A value fails conversion (`CommandLineConversionException`).
- A converted value fails validation (`CommandLineValidationException`).
- A value is generally invalid (`CommandLineArgumentException`).



## Testing

The library provides the `ConfigurationValidator` class that will inspect a `RootCommand` object. The `Validate` method will ensure the following:

- All identifiers are in the proper format
- There are no identifier conflicts in any command pathway
- Handlers are present where needed
- Handlers return a unified result type
- Bindings types are compatible with handler parameter types
- Arity of bindings is valid

> Note
> 
> When a project is built and run in `DEBUG` mode, handler source generators will inject validation. Otherwise, in `RELEASE` validation does not occur so ensure you test your configurations.

```csharp
// Validating configuration in unit tests

[Fact]
public void Configuration_Is_Valid()
{
    var rootCommand = { /* configure */ };

    ConfigurationValidator
        .Validate(rootCommand)
        .Should()
        .BeEmpty();
}
```