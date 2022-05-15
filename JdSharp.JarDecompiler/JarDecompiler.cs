using System.Collections.Generic;
using JdSharp.Core.Decompilers;

namespace JdSharp.JarDecompiler
{
    public class JarDecompiler : IDecompiler
    {
        public IEnumerable<byte[]> AllowedFileSignatures { get; } = new List<byte[]>
        {
            new byte[] { 0xca, 0xfe, 0xba, 0xbe }
        };

        public bool Decompile()
        {
            return true;
        }
    }
}