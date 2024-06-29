using bex.utils;

namespace bex.tree.classes;

public class BexAnyByteClass() : BexByteClass(".", null)
{
    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        //var startOffset = ctx.Offset;
        Exception? ex = null;
        var captured = Bytes.Empty;
        var success = false;

        {
            //captured = new Bytes(ctx.Input.Slice((int) startOffset, 1));
            var buf = await ctx.ReadAsync(cancellationToken);
            //captured = new Bytes(buf.a
            success = true;
            //ctx.Offset += captured.Count;
        }

        return await ValueTask.FromResult(new BexResult(success, -1, Bytes.Empty, this, ex));
    }
}