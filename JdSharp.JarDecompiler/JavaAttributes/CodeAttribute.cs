using System.Collections.Generic;
using System.IO;
using JdSharp.JarDecompiler.ClassFileProperties;
using JdSharp.JarDecompiler.Constants;
using JdSharp.JarDecompiler.Enums;
using JdSharp.JarDecompiler.Utils;

namespace JdSharp.JarDecompiler.JavaAttributes
{
    public class CodeAttribute : BaseAttribute
    {
        public ushort MaxStack { get; }
        public ushort MaxLocals { get; }
        public byte[] Code { get; }
        public ExceptionTable[] ExceptionTables { get; }
        public IDictionary<AttributesEnum, BaseAttribute>? Attributes { get; }

        private CodeAttribute(ushort maxStack, ushort maxLocals, byte[] code,
            ExceptionTable[] exceptionTables,
            IDictionary<AttributesEnum, BaseAttribute>? attributes)
        {
            MaxStack = maxStack;
            MaxLocals = maxLocals;
            Code = code;
            ExceptionTables = exceptionTables;
            Attributes = attributes;
        }

        public static CodeAttribute FromBinaryStream(ref BinaryReader reader, BaseConstant?[] constants)
        {
            ushort maxStack = reader.ReadUInt16();
            ushort maxLocals = reader.ReadUInt16();

            uint codeLenth = reader.ReadUInt32();
            byte[] code = new byte[codeLenth];
                
            reader.ReadBytes((int)codeLenth).CopyTo(code, 0);

            ushort exTableLenth = reader.ReadUInt16();
            ExceptionTable[] exceptionTable = new ExceptionTable[exTableLenth];

            for (int i = 0; i < exTableLenth; i++)
            {
                exceptionTable[i] = ExceptionTable.FromBinaryStream(ref reader);
            }

            ushort attributesCount = reader.ReadUInt16();

            IDictionary<AttributesEnum, BaseAttribute>? attrs = attributesCount == 0
                ? null
                : ClassFileUtils.GetAttributes(attributesCount, ref reader, constants);

            return new CodeAttribute(maxStack, maxLocals, code, exceptionTable, attrs);
        }
    }
}