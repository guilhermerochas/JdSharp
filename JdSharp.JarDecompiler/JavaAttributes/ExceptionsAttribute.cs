using JdSharp.JarDecompiler.Constants;
using JdSharp.JarDecompiler.Extensions;
using System.IO;

namespace JdSharp.JarDecompiler.JavaAttributes
{
    public class ExceptionsAttribute : BaseAttribute
    {
        public string[] ExceptionIndexTable { get; }

        private ExceptionsAttribute(string[] exceptionIndexTable)
        {
            ExceptionIndexTable = exceptionIndexTable;
        }

        public static ExceptionsAttribute FromBinaryStream(ref BinaryReader reader, BaseConstant[] constants)
        {
            ushort exceptionCount = reader.ReadUInt16();
            string[] exceptionTable = new string[exceptionCount];
            for (int i = 0; i < exceptionCount; i++)
            {
                exceptionTable[i] = constants.GetConstantFromUshort(reader.ReadUInt16());
            }

            return new ExceptionsAttribute(exceptionTable);
        }
    }
}