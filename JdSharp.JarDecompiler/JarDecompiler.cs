using System;
using JdSharp.Core;

namespace JdSharp.JarDecompiler
{
    public class JarDecompiler : IBaseDecompiler
    {
        public string FileName { get; set; }
        public string OutFileName { get; set; }
        public bool Decompile()
        {
            throw new NotImplementedException();
        }
    }
}
