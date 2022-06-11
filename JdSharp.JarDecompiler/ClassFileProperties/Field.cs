using JdSharp.JarDecompiler.Constants;
using JdSharp.JarDecompiler.Enums;
using JdSharp.JarDecompiler.Extensions;
using JdSharp.JarDecompiler.JavaAttributes;
using JdSharp.JarDecompiler.Utils;
using System.Collections.Generic;
using System.IO;

namespace JdSharp.JarDecompiler.ClassFileProperties
{
    public class Field
    {
        public AccessFlagEnum[] AccessFlags { get; }
        public string Name { get; }
        public string Descriptor { get; }
        public IDictionary<AttributesEnum, BaseAttribute>? Attributes { get; }

        private Field(AccessFlagEnum[] accessFlags, string name, string descriptor,
            IDictionary<AttributesEnum, BaseAttribute>? attributes)
        {
            AccessFlags = accessFlags;
            Name = name;
            Descriptor = descriptor;
            Attributes = attributes;
        }

        public static Field FromBinaryStream(BinaryReader reader, BaseConstant?[] constants)
        {
            AccessFlagEnum[] accessFlagEnums = ClassFileUtils.GetAccessFlagsFromValue(reader.ReadUInt16());
            string name = constants.GetUft8ConstantFromUshort(reader.ReadUInt16());
            string descriptor = constants.GetUft8ConstantFromUshort(reader.ReadUInt16());
            ushort attributesCount = reader.ReadUInt16();

            IDictionary<AttributesEnum, BaseAttribute>? attributes = attributesCount == 0
                ? null
                : ClassFileUtils.GetAttributes(attributesCount, ref reader, constants);

            return new Field(accessFlagEnums, name, descriptor, attributes);
        }
    }
}