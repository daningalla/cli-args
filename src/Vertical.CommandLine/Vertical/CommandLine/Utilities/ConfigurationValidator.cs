using System.Reflection;
using CommunityToolkit.Diagnostics;
using Vertical.CommandLine.Binding;
using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Invocation;

namespace Vertical.CommandLine.Utilities;

public static class ConfigurationValidator
{
    private sealed class ExceptionComparer : IEqualityComparer<Exception>
    {
        /// <inheritdoc />
        public bool Equals(Exception? x, Exception? y) => StringComparer.Ordinal.Equals(x?.Message, y?.Message);

        /// <inheritdoc />
        public int GetHashCode(Exception obj) => obj.Message.GetHashCode();
    }

    /// <summary>
    /// Validates the given root command configuration.
    /// </summary>
    /// <param name="rootCommand">Root command</param>
    /// <returns>A collection of exceptions that were collected.</returns>
    public static IReadOnlyCollection<Exception> Validate(RootCommand rootCommand)
    {
        Guard.IsNotNull(rootCommand);
        
        var exceptions = new HashSet<Exception>(new ExceptionComparer());
        var commands = new HashSet<Command>();
        var visitor = new PathVisitor<Command>(command => command.Commands, path =>
        {
            foreach (var command in path.Where(commands.Add))
            {
                ValidateCommand(command, exceptions);
            }
            ValidateCommandPath(path, exceptions);
        });
        
        visitor.Visit(rootCommand);
        
        ValidateHandlerSignatures(commands, exceptions);

        return exceptions;
    }

    /// <summary>
    /// Validates the given root command configuration, and throws an exception if any errors
    /// are present.
    /// </summary>
    /// <param name="rootCommand">Root command</param>
    public static void ThrowIfInvalidConfiguration(RootCommand rootCommand)
    {
        var exceptions = Validate(rootCommand);

        if (exceptions.Count == 0)
            return;

        throw exceptions.Count == 1
            ? exceptions.First()
            : new AggregateException(exceptions);
    }

    private static void ValidateCommand(Command command, ICollection<Exception> exceptions)
    {
        ValidateCommandIdentifiers(command, exceptions);
        ValidateUniqueChildCommandIdentifiers(command, exceptions);
        ValidateHandlerRequirement(command, exceptions);
        ValidateArgumentArityConfiguration(command, exceptions);
        ValidateBindingIdentifiers(command, exceptions);
    }

    private static void ValidateCommandPath(IEnumerable<Command> path, ICollection<Exception> exceptions)
    {
        ValidateHandlerSignatures(path, exceptions);
        ValidateParameterBindings(path, exceptions);
        ValidateConverterServices(path, exceptions);
    }

    private static void ValidateConverterServices(IEnumerable<Command> path, ICollection<Exception> exceptions)
    {
        // Validate each type in defined symbols can be converted from a string argument
        
        var bindings = path.SelectMany(command => command.Bindings.Select(binding => (command, value: binding)));

        var converterTypes = new HashSet<Type>(path
            .SelectMany(command => command.Converters)
            .Select(converter => converter.ValueType));
        
        var unsupportedBindings = bindings
            .Where(binding => !binding.value.HasConverter 
                              && !converterTypes.Contains(binding.value.ValueType)
                              && !DefaultValueConverter.CanConvert(binding.value.ValueType));
        
        var typeGroups = unsupportedBindings.GroupBy(binding => binding.value.ValueType);
        
        if (typeGroups.Any())
        {
            exceptions.Add(ConfigurationExceptions.MissingValueConverters(path, typeGroups.ToArray()));
        }        
    }

    private static void ValidateParameterBindings(IEnumerable<Command> path, ICollection<Exception> exceptions)
    {
        // Validate each parameter in handlers can be mapped to a binding symbol or is handled
        // by model binding
        
        var bindings = path.SelectMany(command => command.Bindings);
        var bindingDictionary = bindings.ToDictionary(
            binding => binding.Id,
            binding => binding,
            BindingNameComparer.Instance);
        
        foreach (var command in path.Where(command => command.Handler is not null))
        {
            var method = command.Handler!.Method;
            var parameters = method.GetParameters();
            var unbindableParameters = new List<ParameterInfo>();

            foreach (var parameter in parameters.Where(p => p.ParameterType != typeof(CancellationToken)))
            {
                var modelBinder = command.ModelBinders.FirstOrDefault(binder => 
                    binder.ValueType == parameter.ParameterType);

                if (modelBinder != null)
                {
                    ValidateModelBinder(modelBinder, bindingDictionary.Keys, exceptions);
                    continue;
                }
                
                var bindingName = parameter.GetCustomAttribute<BindingAttribute>()?.BindingId
                                  ?? parameter.Name
                                  ?? string.Empty;

                if (!bindingDictionary.TryGetValue(bindingName, out var symbol))
                {
                    unbindableParameters.Add(parameter);
                    continue;
                }

                if (IsParameterCompatibleWithSymbolType(parameter, symbol))
                    continue;
                
                exceptions.Add(ConfigurationExceptions.IncompatibleParameterType(command, method, parameter, symbol));
            }

            if (unbindableParameters.Count == 0) 
                continue;
            
            exceptions.Add(ConfigurationExceptions.HandlerHasUnbindableParameters(
                command,
                method,
                unbindableParameters));
        }        
    }

    private static void ValidateModelBinder(
        IModelBinder modelBinder,
        IEnumerable<string> bindingIds,
        ICollection<Exception> exceptions)
    {
        var argumentProvider = new ValidatingArgumentProvider(
            new BindingServiceCollection(),
            bindingIds,
            modelBinder.ValueType,
            exceptions);

        modelBinder.BindInstanceBase(argumentProvider);
    }

    private static bool IsParameterCompatibleWithSymbolType(ParameterInfo parameter, CliBindingSymbol binding)
    {
        return binding.ValueType.IsAssignableFrom(parameter.ParameterType);
    }

    private static void ValidateHandlerSignatures(IEnumerable<Command> commands, ICollection<Exception> exceptions)
    {
        // Since commands form a chain, every signature must have the same return type
        // for the Invoke[Async] source generator.
        
        var handledCommands = commands.Where(command => command.Handler is not null);
        var handlerMethods = handledCommands.Select(command => command.Handler!.Method).ToArray();

        if (handlerMethods.Length == 0)
        {
            exceptions.Add(ConfigurationExceptions.NoCommandHandlerInPath(commands));
            return;
        }

        var returnTypeCount = handlerMethods
            .Select(method => method.ReturnType)
            .Distinct()
            .Count();

        if (returnTypeCount != 1)
        {
            exceptions.Add(ConfigurationExceptions.MismatchedHandlerReturnType(handledCommands));
        }
    }

    private static void ValidateArgumentArityConfiguration(Command command, ICollection<Exception> exceptions)
    {
        var argumentBindings = command
            .Bindings
            .Where(binding => binding.SymbolType == CliSymbolType.Argument)
            .ToArray();

        for (var c = 0; c < argumentBindings.Length - 1; c++)
        {
            var arity = argumentBindings[c].Arity;

            if (arity.MinCount > 0 && arity.MinCount == arity.MaxCount)
                continue;
            
            exceptions.Add(ConfigurationExceptions.InvalidArgumentArityOrder(command, argumentBindings));
        }
    }

    private static void ValidateHandlerRequirement(Command command, ICollection<Exception> exceptions)
    {
        // Commands may not be directly handled (only serve as paths to sub commands).
        
        var handlerRequired = command.Bindings.Any(binding => binding.Scope is 
            BindingScope.Self or BindingScope.SelfAndDescendants);

        if (handlerRequired && command.Handler is null)
        {
            exceptions.Add(ConfigurationExceptions.NoCommandHandler(command));
        }
    }

    private static void ValidateUniqueChildCommandIdentifiers(Command command, ICollection<Exception> exceptions)
    {
        // Validate distinct child command identifiers and aliases
        var duplicateChildIdentifierGroupings = command
            .Commands
            .SelectMany(child => child
                .Aliases
                .Append(child.Id)
                .Select(identifier => (child, identifier)))
            .GroupBy(item => item.identifier, item => item.child)
            .Where(grouping => grouping.Count() > 1);

        foreach (var grouping in duplicateChildIdentifierGroupings)
        {
            exceptions.Add(ConfigurationExceptions.DuplicateChildCommandIdentifier(
                grouping.Key,
                grouping.ToArray()));
        }
    }

    private static void ValidateCommandIdentifiers(CliSymbol command, ICollection<Exception> exceptions)
    {
        if (command is RootCommand)
            return;
        
        // Validate naming
        foreach (var identifier in command.Aliases.Append(command.Id))
        {
            if (NamingUtilities.IsValidNonPrefixedIdentifier(identifier))
                continue;

            exceptions.Add(ConfigurationExceptions.InvalidIdentifierName(command, identifier));
        }
    }

    private static void ValidateBindingIdentifiers(Command command, ICollection<Exception> exceptions)
    {
        var bindingNames = command
            .Bindings
            .SelectMany(binding => binding.Identifiers.Select(name => (binding, name)));

        foreach (var (binding, name) in bindingNames)
        {
            var isValid = binding.SymbolType == CliSymbolType.Argument
                ? NamingUtilities.IsValidNonPrefixedIdentifier(name)
                : NamingUtilities.IsValidPrefixedIdentifier(name);

            if (isValid) continue;
            
            exceptions.Add(ConfigurationExceptions.InvalidIdentifierName(
                binding,
                name));
        }
    }
}
