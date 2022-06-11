using System.IO;

namespace JdSharp.JarDecompiler.ClassFileProperties
{
    public class ExceptionTable
    {
        public ushort StartPc { get; }
        public ushort EndPc { get; }
        public ushort HandlerPc { get; }
        public ushort CatchType { get; }

        private ExceptionTable(ushort startPc, ushort endPc, ushort handlerPc, ushort catchType)
        {
            StartPc = startPc;
            EndPc = endPc;
            HandlerPc = handlerPc;
            CatchType = catchType;
        }

        public static ExceptionTable FromBinaryStream(ref BinaryReader reader)
        {
            return new ExceptionTable(
                reader.ReadUInt16(),
                reader.ReadUInt16(),
                reader.ReadUInt16(),
                reader.ReadUInt16());
        }
    }
}