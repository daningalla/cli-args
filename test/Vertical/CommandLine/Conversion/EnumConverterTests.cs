using FluentAssertions;

namespace Vertical.CommandLine.Conversion;

public class EnumConverterTests
{
    [Fact]
    public void Convert_Returns_Enum_Value()
    {
        // arrange
        var converter = new EnumConverter<ConsoleKey>();
        
        // act
        var value = converter.Convert(new ConversionContext<ConsoleKey>("Add", default!));
        
        // assert
        value.Should().Be(ConsoleKey.Add);
        converter.ServiceType.Should().Be(typeof(IValueConverter<ConsoleKey>));
        converter.ValueType.Should().Be(typeof(ConsoleKey));
    }
}