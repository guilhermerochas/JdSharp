namespace JdSharp.Core
{
    public interface IBufferWriter<in TContent>
    {
        public string Write(TContent contet);
    }
}