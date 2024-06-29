using System.Buffers;
using System.IO.Pipelines;

namespace bex.core;

public class BexxOptions(MemoryPool<byte>? pool = default, int bufferSize = 4096, bool leaveOpen = false, int minReadSize = 1024, bool useZeroByteReads = false)
{
    
    // Stream Pipe Reader Options
    public MemoryPool<byte>? Pool { get; internal set; } = pool;
    public int BufferSize { get; internal set; } = bufferSize;
    public bool LeaveOpen { get; internal set; } = leaveOpen;
    public int MinimumReadSize { get; internal set; } = minReadSize;
    public bool UseZeroByteReads { get; internal set; } = useZeroByteReads;
}