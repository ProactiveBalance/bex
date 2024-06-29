using System.Buffers;
using System.Text;
using bex.core;
using bex.parsers;
using bex.tests.data;
using bex.tree;
using bex.tree.classes;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace bex.tests.tree;

public class BexGroupTests
{
    [Fact]
    public async Task Should_Parse_1_Byte_Ok()
    {
        // Arrange
        var bexx = new Bexx("(2F)");
        byte b = 0x2F;
        
        // Act
        var result = await bexx.MatchAsync([b]);
        
        // Assert
        result.Success.Should().BeTrue();

        var mainGroup = result.Matcher as BexGroup;
        var group = mainGroup!.Items.First() as BexGroup;
        group!.Name.Should().BeNull();
    }

    [Fact]
    public async Task Should_Parse_3_Bytes_Ok()
    {
        // Arrange
        var bexx = new Bexx("(2F 01 FF)");
        byte[] bs = [0x2F, 0x01, 0xFF];

        // Act
        var result = await bexx.MatchAsync(bs);

        // Assert
        result.Success.Should().BeTrue();
        var mainGroup = result.Matcher as BexGroup;
        var group = mainGroup.Items.First() as BexGroup;
        group!.Items.Count.December(3);
        group!.Name.Should().BeNull();
    }
    
    [Fact]
    public async Task Should_Build_and_Compile_Group()
    {
        // Arrange
        var bexx = new Bexx("(2F A7 03 00 01 FF)");
        var bytes = new byte[] {0x2F, 0xA7, 0x03, 0x00, 0x01, 0xFF};
        
        // Act
        var bres01 = await bexx.MatchAsync(bytes);
        
        // Assert
        bres01.Success.Should().BeTrue();
        bres01.Captured.Should().BeEquivalentTo(bytes);
    }
    
    [Fact]
    public async Task Should_Build_and_Compile_Named_Group()
    {
        // Arrange
        var bexx = new Bexx("(?<header>2F A7 03 00 01 FF)");
        var bytes = new byte[] {0x2F, 0xA7, 0x03, 0x00, 0x01, 0xFF};
        
        // Act
        var bres01 = await bexx.MatchAsync(new ReadOnlySequence<byte>(bytes));
        
        // Assert
        bres01.Success.Should().BeTrue();
        bres01.Captured.Should().BeEquivalentTo(bytes);
    }

    [Fact]
    public async Task Should_Build_and_Compile_Group_in_Group_all_Literals()
    {
        // Arrange
        var bexx = new Bexx("(?<header>2F A7 (?<version>03 00 01) FF)");
        var bytes = new byte[] {0x2F, 0xA7, 0x03, 0x00, 0x01, 0xFF};
        
        // Act
        var bres01 = await bexx.MatchAsync(new ReadOnlySequence<byte>(bytes));
        
        // Assert
        bres01.Success.Should().BeTrue();
        bres01.Captured.Should().BeEquivalentTo(bytes);
    }
    
    [Fact]
    public async Task Should_Build_and_Compile_Group_in_Group_with_AnyByteGroupClass()
    {
        // Arrange
        var bexx = new Bexx("(?<header> 2F A7 (?<version>. . .) FF )");
        var bytes = new byte[] {0x2F, 0xA7, 0x03, 0x00, 0x01, 0xFF};
        
        // Act
        var bres01 = await bexx.MatchAsync(new ReadOnlySequence<byte>(bytes));
        
        // Assert
        bres01.Success.Should().BeTrue();
        bres01.Captured.Should().BeEquivalentTo(bytes);
    }
    
    [Fact]
    public async Task Should_Build_and_Compile_PDF_Check()
    {
        // Arrange
        await using var smallPdf = ResourceManager.GetEmbeddedResourceAsStream("Small.pdf");
        
        // Act
        var bexx = new Bexx("(?<header>25 50 44 46 2D(?<version>. . .))");
        var bres01 = await bexx.MatchAsync(smallPdf!);
        
        // Assert
        bres01.Success.Should().BeTrue();

        var mainGroup = bexx.Compiled as BexGroup;
        var groupHeader = mainGroup!.Items.First() as BexGroup;
        var groupVersion = groupHeader!.Items[5] as BexGroup;

        groupHeader.Name.Should().Be("header");
        groupVersion!.Name.Should().Be("version");

        bres01.Captured.Count.Should().Be(5);

        //bres01.Group["header"].Group["version"].Result.Captured.As<string>().Should().BeEquivalentTo("1.7");
    }
}