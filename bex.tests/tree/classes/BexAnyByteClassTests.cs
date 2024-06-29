using System.Buffers;
using bex.core;
using bex.parsers;
using bex.tree;
using bex.tree.classes;
using FluentAssertions;

namespace bex.tests.tree.classes;

public class BexAnyByteClassTests
{
    [Fact]
    public async Task Should_Parse_Ok()
    {
        // Arrange
        var bexx = new Bexx(".");
        byte byte01 = 0x77;
        
        // Act
        var result01 = await bexx.MatchAsync([byte01]);

        // Assert
        result01.Success.Should().BeTrue();
        result01.Captured.Should().BeEquivalentTo([byte01]);
    }
    
    [Fact]
    public async Task Should_Build_and_Compile_Literals_and_Any_Ok()
    {
        // Arrange
        var bexx = new Bexx("05 . A1 . 77");
        var bytes01 = new byte[] {0x05, 0xFF, 0xA1, 0x00, 0x77};
        var bytes02 = new byte[] {0x05, 0xFF, 0xA1, 0x00, 0x80};
        
        // Act
        var bres01 = await bexx.MatchAsync(new ReadOnlySequence<byte>(bytes01));
        var bres02 = await bexx.MatchAsync(new ReadOnlySequence<byte>(bytes02));

        // Assert
        bres01.Success.Should().BeTrue();
        bres01.Captured.Should().BeEquivalentTo(bytes01);
        
        bres02.Success.Should().BeFalse();
        bres02.Captured.IsEmpty().Should().BeTrue();
    }
    
    [Fact]
    public async Task Should_Build_and_Compile_Any_and_Literals_Ok()
    {
        // Arrange
        var bexx = new Bexx(". FF . 00 .");
        bexx.Compile();
        var bytes01 = new byte[] {0x05, 0xFF, 0xA1, 0x00, 0x77};
        var bytes02 = new byte[] {0x05, 0xFF, 0xA1, 0x00, 0x80};
        
        // Act
        var bres01 = await bexx.MatchAsync(new ReadOnlySequence<byte>(bytes01));
        var bres02 = await bexx.MatchAsync(new ReadOnlySequence<byte>(bytes02));

        // Assert
        bres01.Success.Should().BeTrue();
        bres01.Captured.Should().BeEquivalentTo(bytes01);
        
        bres02.Success.Should().BeTrue();
        bres02.Captured.Should().BeEquivalentTo(bytes02);
    }
    
}