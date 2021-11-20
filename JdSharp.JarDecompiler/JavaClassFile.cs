using System;
using System.IO;
using System.Linq;
using System.Text;
using JdSharp.Core;
using JdSharp.JarDecompiler.Constants;

namespace JdSharp.JarDecompiler
{
    public class JavaClassFile
    {
        public const uint JavaMagicNumber = 3405691582;

        private uint MagicNumber { get; }
        private ushort MinorVersion { get; }
        private ushort MajorVersion { get; }
        private ushort ConstantPoolCount { get; }
        private BaseConstant[] Constants { get; }

        public JavaClassFile(BinaryReader reader)
        {
            reader.BaseStream.Position = 0x00;
            MagicNumber = reader.ReadUInt32();
            if (MagicNumber != JavaMagicNumber)
                throw new Exception($"Java's magic number doesnt't match with {MagicNumber}");
            MinorVersion = reader.ReadUInt16();
            MajorVersion = reader.ReadUInt16();
            ConstantPoolCount = reader.ReadUInt16();
            Constants = new BaseConstant[ConstantPoolCount];
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder($"Magic Number: {MagicNumber}\n");
            builder.Append($"Minor Versionn: {MinorVersion}\n");
            builder.Append($"Major Version: {MajorVersion}\n");
            builder.Append($"Constant Count: {ConstantPoolCount}\n");
            return builder.ToString();
        }
    }
}