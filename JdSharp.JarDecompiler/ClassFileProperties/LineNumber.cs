namespace JdSharp.JarDecompiler.ClassFileProperties
{
    public class LineNumber
    {
        public ushort StartPc { get; }
        public ushort LineNumberValue { get; }

        public LineNumber(ushort startPc, ushort lineNumberValue)
        {
            StartPc = startPc;
            LineNumberValue = lineNumberValue;
        }
    }
}