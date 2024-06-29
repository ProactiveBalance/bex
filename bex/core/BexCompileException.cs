using Pidgin;

namespace bex.core;

internal class BexCompileException(ParseError<char> error, Exception? inner = null) : ApplicationException(error.RenderErrorMessage(), inner)
{
    internal ParseError<char> Error { get; } = error;

    internal long Offset => Error.ErrorOffset;

    internal SourcePos Pos => Error.ErrorPos;

    // ReSharper disable once InconsistentNaming
    internal bool EOF => Error.EOF;
    
}