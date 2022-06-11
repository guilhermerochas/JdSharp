using JdSharp.JarDecompiler.ClassFileProperties;
using System.IO;

namespace JdSharp.JarDecompiler.JavaAttributes
{
    public class LineNumberTableAttribute : BaseAttribute
    {
        public LineNumber[] LineNumberTable { get; }

        private LineNumberTableAttribute(LineNumber[] lineNumberTable)
        {
            LineNumberTable = lineNumberTable;
        }

        public static LineNumberTableAttribute FromBinaryStrem(ref BinaryReader reader)
        {
            ushort lineNumberLenth = reader.ReadUInt16();
            LineNumber[] lineNumbersTable = new LineNumber[lineNumberLenth];

            for (int i = 0; i < lineNumberLenth; i++)
            {
                lineNumbersTable[i] = new LineNumber(reader.ReadUInt16(), reader.ReadUInt16());
            }

            return new LineNumberTableAttribute(lineNumbersTable);
        }
    }
}