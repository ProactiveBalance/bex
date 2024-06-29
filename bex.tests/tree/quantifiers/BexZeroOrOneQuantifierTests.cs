using bex.tree;
using bex.tree.quantifiers;
using FluentAssertions;

namespace bex.tests.tree.quantifiers;

public class BexZeroOrOneQuantifierTests
{
    [Fact]
    public async Task Should_Parse_Ok()
    {
        // Arrange
        var zooq = new BexZeroOrOneQuantifier(new BexLiteral([0x77]));
        var ctx01 = new BexContext();
        
        // Act
        var result01 = await zooq.MatchAsync(ctx01);

        // Assert
        result01.Success.Should().BeTrue();
    }
    
}