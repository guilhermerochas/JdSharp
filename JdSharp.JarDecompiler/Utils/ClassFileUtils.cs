using System;
using System.IO;
using System.Text;
using JdSharp.JarDecompiler.Constants;

namespace JdSharp.JarDecompiler.Utils
{
    public static class ClassFileUtils
    {
        public static BaseConstant GetConstantType(uint constantTag, ref BinaryReader reader)
        {
            return constantTag switch
            {
                1 => new Utf8Constant(Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()))),
                3 => new IntegerConstant(reader.ReadUInt32()),
                4 => new FloatConstant(reader.ReadInt32()),
                5 => new LongConstant(reader.ReadUInt64()),
                6 => new DoubleConstant(reader.ReadDouble()),
                7 or 19 or 20 => new ClassConstant(reader.ReadUInt16()),
                8 => new StringConstant(reader.ReadUInt16()),
                9 or 10 or 11 or 18 => new MemberRefConstant(reader.ReadUInt16(), reader.ReadUInt16()),
                12 => new NameAndTypeConstant(reader.ReadUInt16(), reader.ReadUInt16()),
                15 => new MethodHandleConstant(reader.ReadByte(), reader.ReadUInt16()),
                16 => new MethodTypeConstant(reader.ReadUInt16()),
                _ => throw new Exception($"Not able to get constant with tag: {constantTag}")
            };
        }

        public static string GetValueFromUshort(this BaseConstant[] constants, ushort thisClass)
        {
            ushort nameIndex = ((ClassConstant)constants[thisClass - 1]).NameIndex;
            return ((Utf8Constant)constants[nameIndex - 1])?.Value;
        }
    }
}