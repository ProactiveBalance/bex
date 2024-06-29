using bex.tree;
using bex.tree.operators;
using Pidgin;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace bex.parsers;

public partial class BexHexParser
{
    private static readonly Parser<char, string> BitwiseAndOperandParser = String("&");
    private static readonly Parser<char, string> BitwiseOrOperandParser = String("|");
    
    private static readonly Parser<char, string> LogicalAndOperatorParser = String("&&");
    private static readonly Parser<char, string> LogicalOrOperatorParser = String("||");
    
    internal static readonly Parser<char, BexLogicalAndOperator> BexLogicalAndOperatorParser = Try(
        from left in BexLiteralsParser.Cast<Bex>().Or(BexGroupParser.Cast<Bex>())
        from token in LogicalAndOperatorParser
        from right in BexLiteralsParser.Cast<Bex>().Or(BexGroupParser.Cast<Bex>())
        select new BexLogicalAndOperator(left, right)
        ).Labelled("LogicalAndOperator");
    
    internal static readonly Parser<char, BexLogicalOrOperator> BexLogicalOrOperatorParser = Try(
        from left in BexLiteralsParser.Cast<Bex>().Or(BexGroupParser.Cast<Bex>())
        from token in LogicalOrOperatorParser
        from right in BexLiteralsParser.Cast<Bex>().Or(BexGroupParser.Cast<Bex>())
        select new BexLogicalOrOperator(left, right)
        ).Labelled("LogicalOrOperator");

    internal static readonly Parser<char, BexOperator> BexOperatorParser = Try(
        OneOf(
            Rec(() => BexLogicalAndOperatorParser.Cast<BexOperator>())
            ,Rec(() => BexLogicalOrOperatorParser.Cast<BexOperator>())
        )).Labelled("Operator");
}
