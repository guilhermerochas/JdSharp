using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JdSharp.JarDecompiler.ClassFileProperties;
using JdSharp.JarDecompiler.Constants;
using JdSharp.JarDecompiler.Enums;
using JdSharp.JarDecompiler.Extensions;
using JdSharp.JarDecompiler.JavaAttributes;

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

        public static AccessFlagEnum[] GetAccessFlagsFromValue(ushort value)
        {
            List<AccessFlagEnum> flags = new List<AccessFlagEnum>();

            foreach (AccessFlagEnum accessFlag in Enum.GetValues(typeof(AccessFlagEnum)))
            {
                if (((ushort)accessFlag & value) != 0)
                {
                    flags.Add(accessFlag);
                }
            }

            return flags.ToArray();
        }

        public static string[] GetInterfaces(uint count, ref BinaryReader reader, BaseConstant?[] constants)
        {
            string[] interfaces = new string[count];

            for (int i = 0; i < count; i++)
            {
                ushort constantIndex = reader.ReadUInt16();
                interfaces[i] = constants.GetConstantFromUshort(constantIndex);
            }

            return interfaces;
        }

        public static Field[] GetFields(ushort count, ref BinaryReader reader, BaseConstant?[] constants)
        {
            Field[] fields = new Field[count];

            for (int i = 0; i < count; i++)
            {
                fields[i] = Field.FromBinaryStream(reader, constants);
            }

            return fields;
        }

        public static Method[] GetMethods(ushort count, ref BinaryReader reader, BaseConstant?[] constants)
        {
            Method[] methods = new Method[count];

            for (int i = 0; i < count; i++)
            {
                methods[i] = Method.FromBinaryStream(ref reader, constants);
            }

            return methods;
        }

        public static IDictionary<AttributesEnum, BaseAttribute> GetAttributes(ushort count, ref BinaryReader reader,
            BaseConstant?[] constants)
        {
            IDictionary<AttributesEnum, BaseAttribute> attrs = new Dictionary<AttributesEnum, BaseAttribute>();
            for (int i = 0; i < count; i++)
            {
                ushort attributeNameIndex = reader.ReadUInt16();
                uint attributeLenth = reader.ReadUInt32();
                BaseConstant? constant = constants[attributeNameIndex - 1];

                if (constant is Utf8Constant utf8Constant)
                {
                    string? info = utf8Constant.Value;

                    if (!string.IsNullOrEmpty(info))
                    {
                        AttributesEnum attributesEnum = Enum.Parse<AttributesEnum>(info);

                        attrs.Add(attributesEnum, GetAttributeFromEnum(ref reader, attributesEnum, attributeLenth, ref constants));
                    }
                }
            }

            return attrs;
        }

        private static BaseAttribute? GetAttributeFromEnum(ref BinaryReader reader, AttributesEnum attributesEnum,
            uint attrLenth, ref BaseConstant?[] constants)
        {
            return attributesEnum switch
            {
                AttributesEnum.ConstantValue => new ConstantValueAttribute(
                    constants.GetUft8ConstantFromUshort(reader.ReadUInt16())),
                AttributesEnum.LineNumberTable => LineNumberTableAttribute.FromBinaryStrem(ref reader),
                AttributesEnum.Code => CodeAttribute.FromBinaryStream(ref reader, constants),
                AttributesEnum.SourceFile => new SourceFileAttribute(
                    Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadUInt16()))),
                AttributesEnum.StackMapTable => new StackMapTableAttribute(reader.ReadBytes((int)attrLenth)),
                _ => throw new Exception($"Not able to get attribute with tag: {attributesEnum}")
            };
        }
    }
}