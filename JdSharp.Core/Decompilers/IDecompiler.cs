using JdSharp.Core.Models;
using System.Collections.Generic;

namespace JdSharp.Core.Decompilers
{
    public interface IDecompiler
    {
        public IEnumerable<byte[]> AllowedFileSignatures { get; }
        public string FileExtension();

        DecompilerResult Decompile(DecompilerOptions options);
    }
}