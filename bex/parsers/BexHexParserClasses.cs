using bex.tree;
using bex.tree.classes;
using bex.tree.operators;
using bex.tree.quantifiers;
using Pidgin;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace bex.parsers;

public partial class BexHexParser
{
    private static readonly Parser<char, string> DotParser = String(".");
    private static readonly Parser<char, string> CaretParser = String("^");
    
    internal static readonly Parser<char, BexAnyByteClass> BexAnyByteClassParser = Try(
        from token1 in DotParser
        select new BexAnyByteClass()
        ).Labelled("AnyByteClass");
    
    internal static readonly Parser<char, BexNegativeByteGroupClass> BexNegativeByteGroupClassParser = Try(
        Brackets(
        from token in CaretParser
        from items in BexLiteralsParser.AtLeastOnce()
        select new BexNegativeByteGroupClass(items)
        )).Labelled("NegativeByteGroupClass");
    
    internal static readonly Parser<char, BexPositiveByteGroupClass> BexPositiveByteGroupClassParser = Try(
        Brackets(
        from items in BexLiteralsParser.AtLeastOnce()
        select new BexPositiveByteGroupClass(items)
        )).Labelled("PositiveByteGroupClass");
    
    internal static readonly Parser<char, BexByteClass> BexByteClassParser = Try(
        OneOf(
            Rec(() => BexAnyByteClassParser.Cast<BexByteClass>())
            ,Rec(() => BexNegativeByteGroupClassParser.Cast<BexByteClass>())
            ,Rec(() => BexPositiveByteGroupClassParser.Cast<BexByteClass>())
        ).Between(SkipWhitespaces)).Labelled("ByteClass");
}
