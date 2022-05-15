using System.IO;

namespace JdSharp.Core
{
    public interface IBufferWriter<in TContent>
    {
        public Stream Write(TContent contet);
    }
}