﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using JdSharp.Core;
using JdSharp.JarDecompiler.ClassFileProperties;
using JdSharp.JarDecompiler.Constants;
using JdSharp.JarDecompiler.Enums;
using JdSharp.JarDecompiler.Extensions;
using JdSharp.JarDecompiler.Utils;

namespace JdSharp.JarDecompiler
{
    public class JavaClassFile
    {
        private const uint JavaMagicNumber = 3405691582;

        private uint MagicNumber { get; }
        private ushort MinorVersion { get; }
        private ushort MajorVersion { get; }
        private ushort ConstantPoolCount { get; }
        private BaseConstant[] Constants { get; }
        private AccessFlagEnum[] AccessFlags { get; }
        private ushort ThisClass { get; }
        private ushort SuperClass { get; }
        private string ClassFileName { get; }
        private string ClassFileType { get; }
        private ushort InterfaceCount { get; }
        private string[]? Interfaces { get; }
        private ushort FieldCount { get; }

        private JavaClassFile(uint magicNumber, ushort minorVersion, ushort majorVersion, ushort constantPoolCount,
            BaseConstant[] baseConstants, AccessFlagEnum[] accessFlags, ushort thisClass, ushort superClass, string classFileName,
            string classFileType, ushort interfaceCount, string[]? interfaces, ushort fieldCount)
        {
            MagicNumber = magicNumber;
            MinorVersion = minorVersion;
            MajorVersion = majorVersion;
            ConstantPoolCount = constantPoolCount;
            Constants = baseConstants;
            AccessFlags = accessFlags;
            ThisClass = thisClass;
            SuperClass = superClass;
            ClassFileName = classFileName;
            ClassFileType = classFileType;
            InterfaceCount = interfaceCount;
            Interfaces = interfaces;
            FieldCount = fieldCount;
        }

        public static JavaClassFile FromBinaryStream(BinaryReader reader)
        {
            reader.BaseStream.Position = 0x00;

            uint magicNumber = reader.ReadUInt32();
            if (magicNumber != JavaMagicNumber)
                throw new Exception($"Java's magic number doesnt't match with {magicNumber}");
            ushort minorVersion = reader.ReadUInt16();
            ushort majorVersion = reader.ReadUInt16();
            ushort constantPoolCount = reader.ReadUInt16();

            BaseConstant[] constants = new BaseConstant[constantPoolCount - 1];
            for (int i = 0; i < constantPoolCount - 1; i++)
            {
                uint tag = reader.ReadByte();
                constants[i] = ClassFileUtils.GetConstantType(tag, ref reader);
            }

            AccessFlagEnum[] accessFlags = ClassFileUtils.GetAccessFlagsFromValue(reader.ReadUInt16());

            ushort thisClass = reader.ReadUInt16();
            string fileName = constants.GetConstantFromUshort(thisClass);
            
            ushort superClass = reader.ReadUInt16();
            string classType = constants.GetConstantFromUshort(superClass);

            ushort interfaceCount = reader.ReadUInt16();

            string[]? interfaces = interfaceCount == 0
                ? null
                : ClassFileUtils.GetInterfaces(interfaceCount, ref reader, constants);

            ushort fieldCount = reader.ReadUInt16();
            Field[]? fields = fieldCount == 0
                ? null
                : ClassFileUtils.GetFields(fieldCount, ref reader, constants);
                
            
            return new JavaClassFile(magicNumber, minorVersion, majorVersion,
                constantPoolCount, constants, accessFlags, thisClass, superClass,
                fileName, classType, interfaceCount, interfaces, fieldCount);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("ClassFile {\n");
            builder.Append($"Magic Number: {MagicNumber}\n");
            builder.Append($"Minor Versionn: {MinorVersion}\n");
            builder.Append($"Major Version: {MajorVersion}\n");
            builder.Append($"Constant Count: {ConstantPoolCount}\n");
            builder.Append($"Access Flags: ");
            AccessFlags.ToList().ForEach(ac =>
            {
                builder.Append(ac.ToStringValue()).Append(' ');
            });
            builder.Append('\n');
            builder.Append($"Class File Name: {ClassFileName}\n");
            builder.Append($"Class File Type: {ClassFileType}\n");
            builder.Append($"Interface Count: {InterfaceCount}\n");
            builder.Append($"Field Count: {FieldCount}\n");
            builder.Append("}\n");
            return builder.ToString();
        }
    }
}