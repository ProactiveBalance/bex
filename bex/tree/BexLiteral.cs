using System.Buffers;
using bex.utils;
using Pidgin;

namespace bex.tree;

public class BexLiteral(byte[] expected) : Bex(BexType.Literal, fetchSize:1)
{
    public byte[] Expected { get; internal set; } = expected;

    
    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default(CancellationToken))
    {
        Exception? ex = null;
        bool success = false;
        var captured = Bytes.Empty;

        var buf = await ctx.ReadAsync(cancellationToken);
        
        if (buf.IsSingleSegment)
        {
            var fixedEnd = buf.GetPosition(Expected.LongLength);
            //reader.read
            
            var mem = buf.Slice(buf.Start, fixedEnd);
            
            if (mem.First.Span.SequenceEqual(Expected))
            {
                captured = new Bytes(Expected);
                success = true;
            }
        }
        else
        {
            throw new NotSupportedException("Reading large Buffers is not implemented!");
        }

        return new BexResult(success, -1, captured, this, ex);
    }

    public override string ToString()
        => Convert.ToHexString(Expected);
}