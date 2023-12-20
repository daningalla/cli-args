using System.Reflection;
using FluentAssertions;

namespace Vertical.CommandLine.Conversion;

public class DefaultConverterTests
{
    private static readonly MethodInfo GetConverterMethod = typeof(DefaultConverterTests)
        .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
        .First(method => method.Name == "GetConverter");
    
    [Theory]
    [InlineData(typeof(bool))]
    [InlineData(typeof(char))]
    [InlineData(typeof(string))]
    [InlineData(typeof(Guid))]
    [InlineData(typeof(sbyte))]
    [InlineData(typeof(short))]
    [InlineData(typeof(int))]
    [InlineData(typeof(long))]
    [InlineData(typeof(Int128))]
    [InlineData(typeof(Half))]
    [InlineData(typeof(float))]
    [InlineData(typeof(double))]
    [InlineData(typeof(decimal))]
    [InlineData(typeof(ushort))]
    [InlineData(typeof(uint))]
    [InlineData(typeof(ulong))]
    [InlineData(typeof(UInt128))]
    [InlineData(typeof(DateTime))]
    [InlineData(typeof(DateTimeOffset))]
    [InlineData(typeof(DateOnly))]
    [InlineData(typeof(TimeOnly))]
    [InlineData(typeof(bool?))]
    [InlineData(typeof(char?))]
    [InlineData(typeof(Guid?))]
    [InlineData(typeof(sbyte?))]
    [InlineData(typeof(short?))]
    [InlineData(typeof(int?))]
    [InlineData(typeof(long?))]
    [InlineData(typeof(Int128?))]
    [InlineData(typeof(Half?))]
    [InlineData(typeof(float?))]
    [InlineData(typeof(double?))]
    [InlineData(typeof(decimal?))]
    [InlineData(typeof(ushort?))]
    [InlineData(typeof(uint?))]
    [InlineData(typeof(ulong?))]
    [InlineData(typeof(UInt128?))]
    [InlineData(typeof(DateTime?))]
    [InlineData(typeof(DateTimeOffset?))]
    [InlineData(typeof(DateOnly?))]
    [InlineData(typeof(TimeOnly?))]
    [InlineData(typeof(FileInfo))]
    [InlineData(typeof(DirectoryInfo))]
    [InlineData(typeof(Uri))]
    [InlineData(typeof(ConsoleKey))]
    public void GetValue_Returns_Instance(Type type)
    {
        // act
        var method = GetConverterMethod.MakeGenericMethod(type);
        var converter = method.Invoke(null, null);
        
        // assert
        DefaultValueConverter.CanConvert(type);
        converter.Should().NotBeNull();
    }

    [Fact]
    public void GetValue_FileInfo_Returns_Expected_Converted_Value()
    {
        // assert
        DefaultValueConverter<FileInfo>.Instance!.Convert(new ConversionContext<FileInfo>("/usr/lib/dotnet",
                null!))
            .FullName
            .Should()
            .Be(new FileInfo("/usr/lib/dotnet").FullName);
    }
    
    [Fact]
    public void GetValue_DirectoryInfo_Returns_Expected_Converted_Value()
    {
        // assert
        DefaultValueConverter<DirectoryInfo>.Instance!.Convert(new ConversionContext<DirectoryInfo>("/var/lib",
                null!))
            .FullName
            .Should()
            .Be(new DirectoryInfo("/var/lib").FullName);
    }
    
    [Fact]
    public void GetValue_Uri_Returns_Expected_Converted_Value()
    {
        // assert
        DefaultValueConverter<Uri>.Instance!.Convert(new ConversionContext<Uri>("https://google.com",
                null!))
            .ToString()
            .Should()
            .Be("https://google.com/");
    }

    private static IValueConverter<T>? GetConverter<T>() => DefaultValueConverter.GetInstanceOrDefault<T>();
}