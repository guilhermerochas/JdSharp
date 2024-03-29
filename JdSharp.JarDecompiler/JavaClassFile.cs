﻿using JdSharp.Core;
using JdSharp.JarDecompiler.BufferWriters;
using JdSharp.JarDecompiler.ClassFileProperties;
using JdSharp.JarDecompiler.Constants;
using JdSharp.JarDecompiler.Enums;
using JdSharp.JarDecompiler.Extensions;
using JdSharp.JarDecompiler.JavaAttributes;
using JdSharp.JarDecompiler.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JdSharp.JarDecompiler
{
    public class JavaClassFile
    {
        public const uint JavaMagicNumber = 3405691582;

        public uint MagicNumber { get; }
        public ushort MinorVersion { get; }
        public ushort MajorVersion { get; }
        public ushort ConstantPoolCount { get; }
        public BaseConstant?[] Constants { get; }
        public AccessFlagEnum[] AccessFlags { get; }
        public ushort ThisClass { get; }
        public ushort SuperClass { get; }
        public string ClassFileName { get; }
        public string ClassFileType { get; }
        public ushort InterfaceCount { get; }
        public string[]? Interfaces { get; }
        public ushort FieldCount { get; }
        public Field[]? Fields { get; }
        public ushort MethodCount { get; }
        public Method[]? Methods { get; }
        public ushort AttributesCount { get; }
        public IDictionary<AttributesEnum, BaseAttribute>? Attributes { get; }

        private JavaClassFile(uint magicNumber, ushort minorVersion, ushort majorVersion, ushort constantPoolCount,
            BaseConstant?[] baseConstants, AccessFlagEnum[] accessFlags, ushort thisClass, ushort superClass,
            string classFileName, string classFileType, ushort interfaceCount, string[]? interfaces,
            ushort fieldCount, Field[]? fields, ushort methodCount, Method[]? methods, ushort attributesCount,
            IDictionary<AttributesEnum, BaseAttribute>? attributes)
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
            Fields = fields;
            MethodCount = methodCount;
            Methods = methods;
            AttributesCount = attributesCount;
            Attributes = attributes;
        }

        public bool IsRecord() => false;
        public bool IsAnnotation() => AccessFlags.Contains(AccessFlagEnum.AccAnnotation);
        public bool IsEnum() => AccessFlags.Contains(AccessFlagEnum.AccEnum);
        public bool IsInterface() => AccessFlags.Contains(AccessFlagEnum.AccInterface);

        public static JavaClassFile FromBinaryStream(BinaryReader reader)
        {
            reader.BaseStream.Position = 0x00;

            uint magicNumber = reader.ReadUInt32();
            if (magicNumber != JavaMagicNumber)
                throw new Exception($"Java's magic number doesnt't match with {magicNumber}");
            ushort minorVersion = reader.ReadUInt16();
            ushort majorVersion = reader.ReadUInt16();
            ushort constantPoolCount = reader.ReadUInt16();

            BaseConstant?[] constants = new BaseConstant?[constantPoolCount - 1];
            for (int i = 0; i < constantPoolCount - 1; i++)
            {
                uint tag = reader.ReadByte();
                constants[i] = ClassFileUtils.GetConstantType(tag, ref reader);

                if (tag is 6)
                {
                    i++;
                    constants[i] = null;
                }
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
            Field[]? fields = fieldCount == 0 ? null : ClassFileUtils.GetFields(fieldCount, ref reader, constants);

            ushort methodCount = reader.ReadUInt16();
            Method[]? methods = methodCount == 0 ? null : ClassFileUtils.GetMethods(methodCount, ref reader, constants);

            ushort attributesCount = reader.ReadUInt16();
            IDictionary<AttributesEnum, BaseAttribute>? attributes = attributesCount == 0
                ? null
                : ClassFileUtils.GetAttributes(attributesCount, ref reader, constants);


            return new JavaClassFile(magicNumber, minorVersion, majorVersion,
                constantPoolCount, constants, accessFlags, thisClass, superClass,
                fileName, classType, interfaceCount, interfaces, fieldCount, fields,
                methodCount, methods, attributesCount, attributes);
        }

        public override string ToString()
        {
            StringBuilder builder = new("ClassFile {\n");
            builder.Append("Magic Number: ").Append(MagicNumber).Append('\n');
            builder.Append("Minor Versionn: ").Append(MinorVersion).Append('\n');
            builder.Append("Major Version: ").Append(MajorVersion).Append('\n');
            builder.Append("Constant Count: ").Append(ConstantPoolCount).Append('\n');
            builder.Append("Access Flags: ");
            AccessFlags.ToList().ForEach(ac => builder.Append(ac.ToStringValue()).Append(' '));
            builder.Append('\n');
            builder.Append("Class File Name: ").Append(ClassFileName).Append('\n');
            builder.Append("Class File Type: ").Append(ClassFileType).Append('\n');
            builder.Append("Field Count: ").Append(FieldCount).Append('\n');
            builder.Append("Method Count ").Append(MethodCount).Append('\n');
            builder.Append("Attribute Count ").Append(AttributesCount).Append('\n');
            builder.Append('}');

            IBufferWriter<JavaClassFile> bufferWriter = new JavaClassWriter();
            byte[]? dataStream = bufferWriter.Write(this);
            builder.Append(dataStream).Append('\n');

            return builder.ToString();
        }
    }
}