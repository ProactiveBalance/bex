using bex.parsers;
using bex.tree;
using bex.tree.operators;
using bex.tree.quantifiers;
using FluentAssertions;
using FluentAssertions.Extensions;
using Pidgin;

using static bex.parsers.BexHexParser;

namespace bex.tests.parsers;

public partial class BexHexParserTests
{

    [Fact]
    public void Should_BexZeroOrOneGreedyQuantifierParser_Parse_Ok()
    {
        //Arrange
        var expression = "3F?";

        // Act
        var result = BexZeroOrOneQuantifierParser.Parse(expression);
        
        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        result.Value.Token.Should().Be("?");
        result.Value.BType.Should().Be(BexType.Quantifier);
        result.Value.Lazy.Should().BeFalse();

        var left = result.Value.LeftOperand;
        left.Should().NotBeNull();
        left.BType.Should().Be(BexType.Literal);
        var leftLiteral = left as BexLiteral;
        leftLiteral.Expected.Should().BeEquivalentTo([0x3F]);
    }
    
    [Fact]
    public void Should_BexZeroOrOneLazyQuantifierParser_Parse_Ok()
    {
        //Arrange
        var expression = "3F??";

        // Act
        var result01 = BexZeroOrOneQuantifierParser.Parse(expression);
        
        // Assert
        result01.Should().NotBeNull();
        result01.Success.Should().BeTrue();

        result01.Value.Token.Should().Be("??");
        result01.Value.BType.Should().Be(BexType.Quantifier);
        result01.Value.Lazy.Should().BeTrue();

        var left = result01.Value.LeftOperand;
        left.Should().NotBeNull();
        left.BType.Should().Be(BexType.Literal);
        var leftLiteral = left as BexLiteral;
        leftLiteral.Expected.Should().BeEquivalentTo([0x3F]);
    }

    [Fact]
    public void Should_BexZeroOrOneGreedyQuantifier_Combined_Parse_Ok()
    {
        //Arrange
        var expression = "2F 3F? 7C";

        // Act
        var result01 = BexHexParser.Compile(expression);
        
        // Assert
        result01.Should().NotBeNull();
        result01.Success.Should().BeTrue();

        var mainGroup = result01.Value;
        var first = mainGroup.Items.First() as BexLiteral;
        var second = mainGroup.Items.ElementAt(1) as BexZeroOrOneQuantifier;
        var zooq = second.LeftOperand as BexLiteral;
        var third = mainGroup.Items.Last() as BexLiteral;

        first.Expected.Should().BeEquivalentTo([0x2F]);
        zooq.Expected.Should().BeEquivalentTo([0x3F]);
        third.Expected.Should().BeEquivalentTo([0x7C]);
    }
    
    [Fact]
    public void Should_BexZeroOrOneGreedyQuantifier_With_Groups_Parse_Ok()
    {
        //Arrange
        var expression = "2F (3F 25)? 7C";

        // Act
        var result01 = BexHexParser.Compile(expression);
        
        // Assert
        result01.Should().NotBeNull();
        result01.Success.Should().BeTrue();

        var mainGroup = result01.Value as BexGroup;
        var first = mainGroup.Items.First() as BexLiteral;
        var second = mainGroup.Items.ElementAt(1) as BexZeroOrOneQuantifier;
        var grp = second.LeftOperand as BexGroup;
        var grp01 = grp.Items.First() as BexLiteral;
        var grp02 = grp.Items.Last() as BexLiteral;
        var third = mainGroup.Items.Last() as BexLiteral;

        first.Expected.Should().BeEquivalentTo([0x2F]);
        grp01.Expected.Should().BeEquivalentTo([0x3F, 0x25]);
        third.Expected.Should().BeEquivalentTo([0x7C]);
    }
}