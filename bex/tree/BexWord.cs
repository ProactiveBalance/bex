using bex.utils;

namespace bex.tree;

public class BexWord() : Bex(BexType.Word)
{
    internal override async ValueTask<BexResult> MatchAsync(BexContext ctx, CancellationToken cancellationToken = default)
    {
        Exception? ex = null;
        bool success = false;
        var captured = Bytes.Empty;
        
        {
            //var c = new Bytes(ctx.Input.Slice((int) ctx.Offset, sizeof(ushort)));

            try
            {
                success = true;
                //captured = c;
                //ctx.Offset += sizeof(ushort);
            }
            catch (Exception e)
            {
                success = false;
                ex = e;
            }
        }

        return await ValueTask.FromResult(new BexResult(success, -1, captured,this, ex));
    }

    public override string ToString()
        => $"(Word)";
}