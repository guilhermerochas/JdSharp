using JdSharp.JarDecompiler.Constants;
using JdSharp.JarDecompiler.Enums;
using JdSharp.JarDecompiler.Extensions;
using JdSharp.JarDecompiler.JavaAttributes;
using JdSharp.JarDecompiler.Utils;
using System.Collections.Generic;
using System.IO;

namespace JdSharp.JarDecompiler.ClassFileProperties
{
    public class Method
    {
        public AccessFlagEnum[] AccessFlagEnums { get; }
        public string Name { get; }
        public string Descriptor { get; }
        public IDictionary<AttributesEnum, BaseAttribute>? Attributes { get; }

        private Method(AccessFlagEnum[] accessFlagEnums, string name, string descriptor,
            IDictionary<AttributesEnum, BaseAttribute>? attributes)
        {
            AccessFlagEnums = accessFlagEnums;
            Name = name;
            Descriptor = descriptor;
            Attributes = attributes;
        }

        public static Method FromBinaryStream(ref BinaryReader reader, BaseConstant?[] constants)
        {
            AccessFlagEnum[] accessFlagsEnum = ClassFileUtils.GetAccessFlagsFromValue(reader.ReadUInt16());
            string name = constants.GetUft8ConstantFromUshort(reader.ReadUInt16());
            string descriptor = constants.GetUft8ConstantFromUshort(reader.ReadUInt16());

            ushort attributesCount = reader.ReadUInt16();
            IDictionary<AttributesEnum, BaseAttribute>? attributes = attributesCount == 0
                ? null
                : ClassFileUtils.GetAttributes(attributesCount, ref reader, constants);

            return new Method(accessFlagsEnum, name, descriptor, attributes);
        }
    }
}