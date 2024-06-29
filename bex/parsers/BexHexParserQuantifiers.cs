using bex.tree;
using bex.tree.operators;
using bex.tree.quantifiers;
using Pidgin;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace bex.parsers;

public partial class BexHexParser
{
    internal static readonly Parser<char, BexZeroOrOneQuantifier> BexZeroOrOneQuantifierParser = Try(
        from left in OneOf(
            Rec(() => BexLiteralsParser.Cast<Bex>()),
            Rec((() => BexGroupParser.Cast<Bex>())))
        from token1 in QuestionMarkParser
        from token2 in QuestionMarkParser.Optional()
        select new BexZeroOrOneQuantifier(left, token2.HasValue)
        ).Labelled("ZeroOrOneQuantifier");
    
    internal static readonly Parser<char, BexQuantifier> BexQuantifierParser = Try(
        OneOf(
            BexZeroOrOneQuantifierParser.Cast<BexQuantifier>()
        )).Labelled("Quantifier");
}
