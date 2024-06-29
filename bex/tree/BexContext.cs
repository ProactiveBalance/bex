using System.Buffers;
using System.IO.Pipelines;
using bex.core;
using bex.utils;

namespace bex.tree;

public struct BexContext
{
    internal PipeReader PipeReader { get; set; }
    
    internal ReadResult PipeReadResult { get; set; }
    
    public BexGroupResultList Group { get; internal set; } = new BexGroupResultList();

    public BexContext(PipeReader reader)
    {
        PipeReader = reader;
    }

    internal async ValueTask<ReadOnlySequence<byte>> ReadAsync(CancellationToken cancellationToken = default)
    {
        PipeReadResult = await PipeReader.ReadAsync(cancellationToken);

        if (PipeReadResult.IsCanceled)
        {
            throw new BexException("Reading canceled!");
        }
        //else if (LastReadResult.IsCompleted)
        //{
           // throw new BexException("Reading completed!");
        //}
        else if (PipeReadResult.Buffer.IsEmpty)
        {
            throw new BexException("Read buffer is Empty!");
        }
        else
        {
            return PipeReadResult.Buffer;
        }
    }

    /*
    internal async ValueTask<bool> TryContainsAsync(ReadOnlySequence<byte> buffer, byte[] bytes)
    {
        var seqReader = new SequenceReader<byte>(buffer);
        if (seqReader.Trys(out byte val))
        {
            
        }
    }
    */

}