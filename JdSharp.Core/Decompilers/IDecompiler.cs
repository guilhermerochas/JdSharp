using System.Collections.Generic;
using JdSharp.Core.Models;

namespace JdSharp.Core.Decompilers
{
    public interface IDecompiler
    {
        public IEnumerable<byte[]> AllowedFileSignatures { get; }
        public string FileExtension();

        DecompilerResult Decompile(DecompilerOptions options);
    }
}