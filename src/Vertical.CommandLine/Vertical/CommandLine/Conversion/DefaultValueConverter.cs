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
            [typeof(TimeSpan)] = () => new ValueConverter<TimeSpan>(TimeSpan.Parse),
            
            // Nullable variants
            [typeof(char?)] = () => new ValueConverter<char?>(str => char.Parse(str)),
            [typeof(Guid?)] = () => new ValueConverter<Guid?>(str => Guid.Parse(str)),
            
            [typeof(sbyte?)] = () => new ValueConverter<sbyte?>(str => sbyte.Parse(str)),
            [typeof(short?)] = () => new ValueConverter<short?>(str => short.Parse(str)),
            [typeof(int?)] = () => new ValueConverter<int?>(str => int.Parse(str)),
            [typeof(long?)] = () => new ValueConverter<long?>(str => long.Parse(str)),
            [typeof(Int128?)] = () => new ValueConverter<Int128?>(str => Int128.Parse(str)),
            [typeof(Half?)] = () => new ValueConverter<Half?>(str => Half.Parse(str)),
            [typeof(float?)] = () => new ValueConverter<float?>(str => float.Parse(str)),
            [typeof(double?)] = () => new ValueConverter<double?>(str => double.Parse(str)),
            [typeof(decimal?)] = () => new ValueConverter<decimal?>(str => decimal.Parse(str)),

            [typeof(bool?)] = () => new ValueConverter<bool?>(str => bool.Parse(str)),
            [typeof(ushort?)] = () => new ValueConverter<ushort?>(str => ushort.Parse(str)),
            [typeof(uint?)] = () => new ValueConverter<uint?>(str => uint.Parse(str)),
            [typeof(ulong?)] = () => new ValueConverter<ulong?>(str => ulong.Parse(str)),
            [typeof(UInt128?)] = () => new ValueConverter<UInt128?>(str => UInt128.Parse(str)),
            
            [typeof(DateTime?)] = () => new ValueConverter<DateTime?>(str => DateTime.Parse(str)), 
            [typeof(DateTimeOffset?)] = () => new ValueConverter<DateTimeOffset?>(str => DateTimeOffset.Parse(str)), 
            [typeof(DateOnly?)] = () => new ValueConverter<DateOnly?>(str => DateOnly.Parse(str)), 
            [typeof(TimeOnly?)] = () => new ValueConverter<TimeOnly?>(str => TimeOnly.Parse(str)),
            [typeof(TimeSpan?)] = () => new ValueConverter<TimeSpan?>(str => TimeSpan.Parse(str)),
            
            [typeof(FileInfo)] = () => new ValueConverter<FileInfo>(str => new FileInfo(str)),
            [typeof(DirectoryInfo)] = () => new ValueConverter<DirectoryInfo>(str => new DirectoryInfo(str)),
            [typeof(Uri)] = () => new ValueConverter<Uri>(str => new Uri(str, UriKind.RelativeOrAbsolute))
        };

    internal static bool CanConvert(Type type) => type.IsEnum || FactoryDictionary.ContainsKey(type); 

    internal static IValueConverter<T>? GetInstanceOrDefault<T>()
    {
        var targetType = typeof(T);

        return targetType.IsEnum
            ? new EnumConverter<T>()
            : (IValueConverter<T>?)FactoryDictionary.GetValueOrDefault(targetType)?.Invoke();
    }
}