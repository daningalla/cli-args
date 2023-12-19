# vertical-cli

A command-line argument parser.

> Note
>
> This library was made for internal use and as an experiment/learning path for source generator development. Updates, enhancements, and patches are not guaranteed. Use at your own risk. This library was motivated by and shares a lot of semantics with System.CommandLine.

## Overview

This library parses command line arguments. Top level features include:

- Parses POSIX, GNU and Microsoft style option syntax.
- Provides strongly-typed, mapped arguments to application defined delegates.
- No runtime reflection/is AOT trim friendly and optimized using source generation.
- Provides robust configuration validation for unit testing.
- Supports hierarchical routing using sub-command definitions.
- Provides interfaces for custom value conversion, validation, and complex type binding.

## Quick setup

```
$ dotnet add package vertical-cli --pre
```

```csharp
using Vertical.CommandLine;

var rootCommand = new RootCommand
{
    Bindings = 
    {
        new Argument<FileInfo>("source", arity: Arity.One),
        new Argument<FileInfo>("dest", arity: Arity.One),
        new Option<string?>("--compression")
    },
    Handler = async (
        FileInfo source, 
        FileInfo dest, 
        string? compression, 
        CancellationToken cancelToken) => 
        {
            using var sourceStream = File.OpenRead(source);
            using var destStream = "gzip" == compression
                ? new GZipStream(File.OpenWrite(dest))
                : File.OpenWrite(dest);

            await sourceStream.CopyToAsync(destStream);
            Console.WriteLine($"File {source} copied.");            
        }
};

await rootCommand.InvokeAsync(args, CancellationToken.None);
```

## Getting started

### Commands

_Commands_ are specific actions an application can take. For example, using .Net CLI tooling, you can create a project by entering `dotnet new classlib -n MyProject`. While `dotnet` is the name of executable program, `new` is a command that instructs the tooling to create a new project. In vertical-cli terminology, the _root_ command represents the executable program itself (`dotnet`), and `new` would be defined as a _sub_-command of the root command. Every application must define a root command.

Even though this example is incomplete, `dotnet new` could be setup as such:

```csharp
// The main program
var rootCommand = new RootCommand
{
    // The "new" sub-program
    Commands = { new Command("new") }
};
```

### Options & arguments

In the `dotnet new classlib -n MyProject` example, `classlib` is an unnamed argument, and `-n MyProject` is a named option with an operand value for the `new` command. The parser supports POSIX, GNU, and Microsoft style option formats:

- POSIX style character switches with or without boolean operand, e.g. `-s`, `-s true`, `-s=true`, `-s:true` are equvalent.
- POSIX style character options with an operand, e.g. `-p /usr/lib`, `-p=/usr/lib`, `-p:/usr/lib` are equivalent.
- GNU style options with/without an operand, q.g. `--no-restore`, ``