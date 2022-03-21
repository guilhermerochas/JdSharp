using System.Collections.Generic;

namespace JdSharp.JarDecompiler.BufferWriters
{
    public class MethodWriter
    {
        public string Type { get; set; } = string.Empty;
        public List<Argument> Arguments { get; init; } = new();
        public int ArrayDepth { get; set; }
    }

    public class Argument
    {
        public string Name { get; set; }
        public int ArrayDepth { get; set; }
    }
}