namespace Vertical.CommandLine.Conversion;

internal static class DefaultValueConverter
{
    private static readonly Dictionary<Type, Func<IValueConverter>> FactoryDictionary =
        new()
        {
            [typeof(char)] = () => new DelegateConverter<char>(char.Parse),
            [typeof(string)] = () => new DelegateConverter<string>(str => str),
            [typeof(Guid)] = () => new DelegateConverter<Guid>(Guid.Parse),
            
            [typeof(sbyte)] = () => new DelegateConverter<sbyte>(sbyte.Parse),
            [typeof(short)] = () => new DelegateConverter<short>(short.Parse),
            [typeof(int)] = () => new DelegateConverter<int>(int.Parse),
            [typeof(long)] = () => new DelegateConverter<long>(long.Parse),
            [typeof(Int128)] = () => new DelegateConverter<Int128>(Int128.Parse),
            [typeof(Half)] = () => new DelegateConverter<Half>(Half.Parse),
            [typeof(float)] = () => new DelegateConverter<float>(float.Parse),
            [typeof(double)] = () => new DelegateConverter<double>(double.Parse),
            [typeof(decimal)] = () => new DelegateConverter<decimal>(decimal.Parse),

            [typeof(bool)] = () => new DelegateConverter<bool>(bool.Parse),
            [typeof(ushort)] = () => new DelegateConverter<ushort>(ushort.Parse),
            [typeof(uint)] = () => new DelegateConverter<uint>(uint.Parse),
            [typeof(ulong)] = () => new DelegateConverter<ulong>(ulong.Parse),
            [typeof(UInt128)] = () => new DelegateConverter<UInt128>(UInt128.Parse),
            
            [typeof(DateTime)] = () => new DelegateConverter<DateTime>(DateTime.Parse), 
            [typeof(DateTimeOffset)] = () => new DelegateConverter<DateTimeOffset>(DateTimeOffset.Parse), 
            [typeof(DateOnly)] = () => new DelegateConverter<DateOnly>(DateOnly.Parse), 
            [typeof(TimeOnly)] = () => new DelegateConverter<TimeOnly>(TimeOnly.Parse),
            [typeof(TimeSpan)] = () => new DelegateConverter<TimeSpan>(TimeSpan.Parse),
            
            // Nullable variants
            [typeof(char?)] = () => new DelegateConverter<char?>(str => char.Parse(str)),
            [typeof(Guid?)] = () => new DelegateConverter<Guid?>(str => Guid.Parse(str)),
            
            [typeof(sbyte?)] = () => new DelegateConverter<sbyte?>(str => sbyte.Parse(str)),
            [typeof(short?)] = () => new DelegateConverter<short?>(str => short.Parse(str)),
            [typeof(int?)] = () => new DelegateConverter<int?>(str => int.Parse(str)),
            [typeof(long?)] = () => new DelegateConverter<long?>(str => long.Parse(str)),
            [typeof(Int128?)] = () => new DelegateConverter<Int128?>(str => Int128.Parse(str)),
            [typeof(Half?)] = () => new DelegateConverter<Half?>(str => Half.Parse(str)),
            [typeof(float?)] = () => new DelegateConverter<float?>(str => float.Parse(str)),
            [typeof(double?)] = () => new DelegateConverter<double?>(str => double.Parse(str)),
            [typeof(decimal?)] = () => new DelegateConverter<decimal?>(str => decimal.Parse(str)),

            [typeof(bool?)] = () => new DelegateConverter<bool?>(str => bool.Parse(str)),
            [typeof(ushort?)] = () => new DelegateConverter<ushort?>(str => ushort.Parse(str)),
            [typeof(uint?)] = () => new DelegateConverter<uint?>(str => uint.Parse(str)),
            [typeof(ulong?)] = () => new DelegateConverter<ulong?>(str => ulong.Parse(str)),
            [typeof(UInt128?)] = () => new DelegateConverter<UInt128?>(str => UInt128.Parse(str)),
            
            [typeof(DateTime?)] = () => new DelegateConverter<DateTime?>(str => DateTime.Parse(str)), 
            [typeof(DateTimeOffset?)] = () => new DelegateConverter<DateTimeOffset?>(str => DateTimeOffset.Parse(str)), 
            [typeof(DateOnly?)] = () => new DelegateConverter<DateOnly?>(str => DateOnly.Parse(str)), 
            [typeof(TimeOnly?)] = () => new DelegateConverter<TimeOnly?>(str => TimeOnly.Parse(str)),
            [typeof(TimeSpan?)] = () => new DelegateConverter<TimeSpan?>(str => TimeSpan.Parse(str)),
            
            [typeof(FileInfo)] = () => new DelegateConverter<FileInfo>(str => new FileInfo(str)),
            [typeof(DirectoryInfo)] = () => new DelegateConverter<DirectoryInfo>(str => new DirectoryInfo(str)),
            [typeof(Uri)] = () => new DelegateConverter<Uri>(str => new Uri(str, UriKind.RelativeOrAbsolute))
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