using bex.core;
using bex.tree;
using FluentAssertions;

namespace bex.tests.tree;

public class BexLiteralTests
{
    [Fact]
    public async Task Should_Parse_One_Byte()
    {
        // Arrange
        var bexx = new Bexx("2F");
        byte b = 0x2F;
        
        // Act
        var result = await bexx.MatchAsync([b]);
        
        // Assert
        result.Success.Should().BeTrue();

        var mainGroup = bexx.Compiled;
        var literal = mainGroup!.Items.First() as BexLiteral;
        literal.Should().NotBeNull("");
        literal.Should().BeOfType<BexLiteral>();
        literal!.IsLiteral().Should().BeTrue();
        literal!.Expected.Should().BeEquivalentTo([0x2F]);

        literal.Previous.Should().BeNull();
        literal.Next.Should().BeNull();
        literal.Parent.Should().Be(mainGroup);

        result.Captured.SequenceEqual([b]).Should().BeTrue();
    }
    
    
    [Fact]
    public async Task Should_Parse_Five_Bytes()
    {
        // Arrange
        var bexx = new Bexx("2F 21 06 77 03");
        byte[] bytes = [0x2F, 0x21, 0x06, 0x77, 0x03];
        
        // Act
        var result = await bexx.MatchAsync(bytes);
        
        // Assert
        result.Success.Should().BeTrue();

        var mainGroup = bexx.Compiled;
        var lit01 = mainGroup!.Items.First() as BexLiteral;
        lit01.Should().NotBeNull("");
        lit01.Should().BeOfType<BexLiteral>();
        lit01!.IsLiteral().Should().BeTrue();
        lit01!.Expected.Should().BeEquivalentTo([0x2F]);

        lit01.Previous.Should().BeNull();
        lit01.Next.Should().BeNull();
        lit01.Parent.Should().BeNull();

        var lit02 = mainGroup.Items[1] as BexLiteral;
        lit02!.Expected.Should().BeEquivalentTo([0x21]);
        
        var lit03 = mainGroup.Items[2] as BexLiteral;
        lit03!.Expected.Should().BeEquivalentTo([0x06]);
        
        var lit04 = mainGroup.Items[3] as BexLiteral;
        lit04!.Expected.Should().BeEquivalentTo([0x77]);
        
        var lit05 = mainGroup.Items[4] as BexLiteral;
        lit05!.Expected.Should().BeEquivalentTo([0x03]);

        result.Captured.SequenceEqual(bytes).Should().BeTrue();
    }
    
    
}