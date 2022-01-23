using System.IO;
using JdSharp.JarDecompiler.Constants;
using JdSharp.JarDecompiler.Enums;
using JdSharp.JarDecompiler.Extensions;
using JdSharp.JarDecompiler.Utils;

namespace JdSharp.JarDecompiler.ClassFileProperties
{
    public class Class
    {
        public string InnerClassInfo { get; }
        public string OuterClassInfo { get; }
        public string InnerName { get; }
        public AccessFlagEnum[] InnerClassAccess { get; }

        private Class(string innerClassInfo, string outerClassInfo, string innerName, AccessFlagEnum[] innerClassAccess)
        {
            InnerClassInfo = innerClassInfo;
            OuterClassInfo = outerClassInfo;
            InnerName = innerName;
            InnerClassAccess = innerClassAccess;
        }

        public static Class FromBinaryStream(ref BinaryReader reader, BaseConstant[] constants)
        {
            string innerClassInfo = constants.GetConstantFromUshort(reader.ReadUInt16());
            string outerClassInfo = constants.GetConstantFromUshort(reader.ReadUInt16());
            string innerName = constants.GetUft8ConstantFromUshort(reader.ReadUInt16());
            AccessFlagEnum[] innerClassAccess = ClassFileUtils.GetAccessFlagsFromValue(reader.ReadUInt16());
            
            return new Class(innerClassInfo, outerClassInfo, innerName, innerClassAccess);
        }
    }
}