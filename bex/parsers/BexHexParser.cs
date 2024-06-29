using bex.tree;
using bex.tree.operators;
using Pidgin;
using Pidgin.Expression;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace bex.parsers;

public partial class BexHexParser
{
    internal static readonly Parser<char, char> LeftBraceParser = Char('{');
    internal static readonly Parser<char, char> RightBraceParser = Char('}');
    internal static readonly Parser<char, char> LeftBracketParser = Char('[');
    internal static readonly Parser<char, char> RightBracketParser = Char(']');
    internal static readonly Parser<char, char> LeftParenthesesParser = Char('(');
    internal static readonly Parser<char, char> RightParenthesesParser = Char(')');
    internal static readonly Parser<char, char> LesserThanParser = Char('<');
    internal static readonly Parser<char, char> GreaterThanParser = Char('>');
    internal static readonly Parser<char, char> QuestionMarkParser = Char('?');
    internal static readonly Parser<char, char> DoubleQuoteParser = Char('"');
    internal static readonly Parser<char, char> ColonParser = Char(':');
    internal static readonly Parser<char, char> PlusParser = Char('+');
    internal static readonly Parser<char, char> CommaParser = Char(',');
    internal static readonly Parser<char, char> SpaceParser = Char(' ');
   
    internal static readonly Parser<char, char> HexCharParser = Token(char.IsAsciiHexDigit);

    internal static Parser<char, T> Tok<T>(Parser<char, T> token)
        => Try(token).Before(SkipWhitespaces);

    internal static Parser<char, string> Tok(string token)
        => Tok(String(token));

    internal static Parser<char, T> Parenthesised<T>(Parser<char, T> parser)
        => parser.Between(LeftParenthesesParser, RightParenthesesParser);

    internal static Parser<char, T> Brackets<T>(Parser<char, T> parser)
        => parser.Between(LeftBracketParser, RightBracketParser);
    

    internal static readonly Parser<char, string> HexStringByteParser = (
        from first in HexCharParser
        from second in HexCharParser
        select new string([first, second])).Labelled("HexStringByte");

    internal static readonly Parser<char, string> HexStringParser = (
        from first in HexCharParser.AtLeastOnceString()
        select first).Labelled("HexString");

    
    internal static readonly Parser<char, BexLiteral> BexLiteralsParser = Try(
        from hexStrByte in HexStringByteParser.Between(SkipWhitespaces)
        select new BexLiteral(Convert.FromHexString(hexStrByte))
        ).Labelled("Literal");
    
    internal static readonly Parser<char, Bex> BexAllItemsParser = Try(
        OneOf(
            Rec(() => BexOperatorParser.Cast<Bex>())
            ,Rec(() => BexQuantifierParser.Cast<Bex>())
            ,Rec(() => BexByteClassParser.Cast<Bex>())
            ,Rec(() => BexFunctionParser.Cast<Bex>())
            ,Rec(() => BexLiteralsParser.Cast<Bex>())
            ,Rec(() => BexGroupParser.Cast<Bex>())
            )).Labelled("AllItems");
    
    internal static readonly Parser<char, string> GroupNameParser = Try(
        from qm in QuestionMarkParser 
        from lt in LesserThanParser
        from fl in Token(char.IsLetter)
        from r in Token(char.IsLetterOrDigit).ManyString()
        from gt in GreaterThanParser
        select $"{fl}{r}"
        ).Labelled("GroupName");

    
    internal static readonly Parser<char, BexGroup> BexGroupParser = Try(Parenthesised(
        from gn in GroupNameParser.Optional()
        from items in Rec(() => BexAllItemsParser.AtLeastOnce())
        select new BexGroup(items, gn.GetValueOrDefault())
        )).Labelled("Group");
      
    internal static readonly Parser<char, BexGroup> BexMainGroupParser = Try(
        from items in Rec(() => BexAllItemsParser.AtLeastOnce())
        select new BexGroup(items, "main")
        ).Labelled("MainGroup");
    
    public static Result<char, BexGroup> Compile(string expressions) => BexMainGroupParser.Parse(expressions);
}