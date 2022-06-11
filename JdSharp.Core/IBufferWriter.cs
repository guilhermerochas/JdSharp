namespace JdSharp.Core
{
    public interface IBufferWriter<in TContent>
    {
        public byte[] Write(TContent contet);
    }
}