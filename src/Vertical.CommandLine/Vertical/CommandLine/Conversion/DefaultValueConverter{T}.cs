namespace Vertical.CommandLine.Conversion;

internal static class DefaultValueConverter<T>
{
    internal static IValueConverter<T>? Instance { get; } = DefaultValueConverter.GetInstanceOrDefault<T>();
}