using System.Collections.Generic;

namespace JdSharp.Core.Decompilers
{
    public interface IDecompiler
    {
        public IEnumerable<byte[]> AllowedFileSignatures { get; }

        bool Decompile();
    }
}