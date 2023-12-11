namespace Vertical.CommandLine.Conversion;

internal static class DefaultValueConverter
{
    private static readonly Dictionary<Type, Func<IValueConverter>> FactoryDictionary =
        new()
        {
            [typeof(char)] = () => new ValueConverter<char>(char.Parse),
            [typeof(string)] = () => new ValueConverter<string>(str => str),
            [typeof(Guid)] = () => new ValueConverter<Guid>(Guid.Parse),
            
            [typeof(sbyte)] = () => new ValueConverter<sbyte>(sbyte.Parse),
            [typeof(short)] = () => new ValueConverter<short>(short.Parse),
            [typeof(int)] = () => new ValueConverter<int>(int.Parse),
            [typeof(long)] = () => new ValueConverter<long>(long.Parse),
            [typeof(Int128)] = () => new ValueConverter<Int128>(Int128.Parse),
            [typeof(Half)] = () => new ValueConverter<Half>(Half.Parse),
            [typeof(float)] = () => new ValueConverter<float>(float.Parse),
            [typeof(double)] = () => new ValueConverter<double>(double.Parse),
            [typeof(decimal)] = () => new ValueConverter<decimal>(decimal.Parse),

            [typeof(bool)] = () => new ValueConverter<bool>(bool.Parse),
            [typeof(ushort)] = () => new ValueConverter<ushort>(ushort.Parse),
            [typeof(uint)] = () => new ValueConverter<uint>(uint.Parse),
            [typeof(ulong)] = () => new ValueConverter<ulong>(ulong.Parse),
            [typeof(UInt128)] = () => new ValueConverter<UInt128>(UInt128.Parse),
            
            [typeof(DateTime)] = () => new ValueConverter<DateTime>(DateTime.Parse), 
            [typeof(DateTimeOffset)] = () => new ValueConverter<DateTimeOffset>(DateTimeOffset.Parse), 
            [typeof(DateOnly)] = () => new ValueConverter<DateOnly>(DateOnly.Parse), 
            [typeof(TimeOnly)] = () => new ValueConverter<TimeOnly>(TimeOnly.Parse),
            
            [typeof(FileInfo)] = () => new ValueConverter<FileInfo>(str => new FileInfo(str)),
            [typeof(DirectoryInfo)] = () => new ValueConverter<DirectoryInfo>(str => new DirectoryInfo(str)),
            [typeof(Uri)] = () => new ValueConverter<Uri>(str => new Uri(str, UriKind.RelativeOrAbsolute))
        };

    internal static bool CanConvert(Type type) => FactoryDictionary.ContainsKey(type); 

    internal static IValueConverter<T>? GetInstanceOrDefault<T>()
    {
        var targetType = typeof(T);

        return targetType.IsEnum
            ? new EnumConverter<T>()
            : (IValueConverter<T>?)FactoryDictionary.GetValueOrDefault(targetType)?.Invoke();
    }
}