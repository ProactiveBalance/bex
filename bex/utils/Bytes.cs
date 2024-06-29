namespace bex.utils;

public class Bytes : List<byte>
{
    public static Bytes Empty => new Bytes();


    public Bytes(byte[]? bArr = null)
    {
        if (bArr != null)
        {
            this.AddRange(bArr);
        }
    }

    public Bytes(ReadOnlySpan<byte> ros)
    {
        this.AddRange(ros.ToArray());
    }

    public bool IsEmpty()
    {
        return !this.Any();
    }
}