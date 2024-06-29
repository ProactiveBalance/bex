using bex.tree;
using bex.tree.operators;
using bex.tree.quantifiers;
using Pidgin;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace bex.parsers;

public partial class BexHexParser
{
    internal static readonly Parser<char, BexZeroOrOneQuantifier> BexAsStringParser = (
        from left in BexLiteralsParser.Cast<Bex>().Or(BexGroupParser.Cast<Bex>())
        from token1 in QuestionMarkParser
        from token2 in QuestionMarkParser.Optional()
        select new BexZeroOrOneQuantifier(left, token2.HasValue)
        ).Labelled("ZeroOrOneQuantifier");
    
    internal static readonly Parser<char, string> FunctionNameParser = Try(
        from fl in Token(char.IsLetter)
        from r in Token(char.IsLetterOrDigit).ManyString()
        select $"{fl}{r}"
        );

    internal static readonly Parser<char, BexFunction> BexFunctionParser = Try(
        from fn in FunctionNameParser
        from lp in LeftParenthesesParser
        from args in HexStringParser.Separated(CommaParser)
        from rp in RightParenthesesParser
        select new BexFunction(fn, args.ToArray())
        ).Labelled("Function");

}
