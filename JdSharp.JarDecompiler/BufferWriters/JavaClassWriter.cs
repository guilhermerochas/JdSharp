﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JdSharp.Core;
using JdSharp.JarDecompiler.Enums;
using JdSharp.JarDecompiler.Extensions;
using JdSharp.JarDecompiler.Utils;

namespace JdSharp.JarDecompiler.BufferWriters
{
    public class JavaClassWriter : IBufferWriter<JavaClassFile>
    {
        private readonly List<string> _importsList = new();
        private readonly StringBuilder _contentBuilder = new("\n");
        private JavaClassFile _javaClass = null!;
        private readonly Encoding _encoding = Encoding.UTF8;

        public const string JavaLangPackageDefinition = "Ljava/lang/";
        public const string ConstructorSpecialMethodName = "<init>";
        public const string EnumValuesContant = "$VALUES";

        private const int ClassAllowed = (int)(AccessFlagEnum.AccPublic | AccessFlagEnum.AccProtected |
                                               AccessFlagEnum.AccAbstract | AccessFlagEnum.AccPrivate |
                                               AccessFlagEnum.AccStatic | AccessFlagEnum.AccFinal |
                                               AccessFlagEnum.AccStrict);

        private const int FieldAllowed =
            (int)(AccessFlagEnum.AccPublic | AccessFlagEnum.AccProtected | AccessFlagEnum.AccPrivate |
                  AccessFlagEnum.AccStatic | AccessFlagEnum.AccFinal |
                  AccessFlagEnum.AccVarargs | AccessFlagEnum.AccBridge);

        private const int MethodAllowed = (int)(AccessFlagEnum.AccPublic | AccessFlagEnum.AccProtected |
                                                AccessFlagEnum.AccPrivate | AccessFlagEnum.AccNative |
                                                AccessFlagEnum.AccFinal | AccessFlagEnum.AccStatic |
                                                AccessFlagEnum.AccSuper | AccessFlagEnum.AccAbstract);

        public Stream Write(JavaClassFile content)
        {
            _javaClass = content;

            ParseClassStructure();
            ParseFields();
            ParseMethods();

            return new MemoryStream(
                _encoding.GetBytes(string.Join("\n", _importsList) + '\n' + _contentBuilder.Append('}')));
        }

        private void ParseClassStructure()
        {
            var accessFlagEnums = _javaClass.AccessFlags.OrderBy(flags => (int)flags);
            foreach (var flag in accessFlagEnums)
            {
                if (((int)flag & ClassAllowed) != 0)
                {
                    string flagToClass = flag.ToJavaClass();

                    if (string.IsNullOrEmpty(flagToClass))
                    {
                        continue;
                    }

                    _contentBuilder.Append(flagToClass).Append(' ');
                }
            }

            if (_javaClass.IsInterface())
            {
                if (_javaClass.IsAnnotation())
                {
                    _contentBuilder.Append(AccessFlagEnum.AccAnnotation.ToJavaClass());
                }

                _contentBuilder.Append(AccessFlagEnum.AccInterface.ToJavaClass());
            }
            else if (_javaClass.IsEnum())
            {
                _contentBuilder.Append(AccessFlagEnum.AccEnum.ToJavaClass());
            }
            else if (_javaClass.IsRecord())
            {
                _contentBuilder.Append("record");
            }
            else
            {
                _contentBuilder.Append("class");
            }

            _contentBuilder.Append(' ').Append(_javaClass.ClassFileName).Append(" {").Append('\n');
        }

        private void ParseFields()
        {
            var fields = _javaClass.Fields;

            if (fields is null)
                return;

            bool isEnum = _javaClass.IsEnum();

            foreach (var field in fields)
            {
                _contentBuilder.Append('\t');

                if (isEnum)
                {
                    if (field.Name is EnumValuesContant)
                    {
                        continue;
                    }

                    _contentBuilder.Append(field.Name).Append(",\n");
                    continue;
                }

                var flags = field.AccessFlags.OrderBy(flag => (int)flag);
                foreach (AccessFlagEnum flag in flags)
                {
                    if (((int)flag & FieldAllowed) != 0)
                    {
                        string flatToField = flag.ToJavaField();

                        if (string.IsNullOrEmpty(flatToField))
                        {
                            continue;
                        }

                        _contentBuilder.Append(flatToField).Append(' ');
                    }
                }

                string descriptor = ParserUtils.FieldDescriptorToJava(field.Descriptor);
                AppendObjectWithPackage(descriptor, field.Descriptor);

                _contentBuilder.Append(' ').Append(field.Name).Append(";\n");
            }

            if (isEnum)
            {
                _contentBuilder.Length -= 3;
            }

            _contentBuilder.Append('\n');
        }

        private void ParseMethods()
        {
            var methods = _javaClass.Methods;

            if (methods is null)
                return;

            bool isEnum = _javaClass.IsEnum();

            if (isEnum)
                return;

            foreach (var method in methods)
            {
                if (method.Name is "<clinit>")
                {
                    continue;
                }

                _contentBuilder.Append('\t');
                var flags = method.AccessFlagEnums.OrderBy(flag => (int)flag);

                foreach (AccessFlagEnum flag in flags)
                {
                    if (((int)flag & MethodAllowed) != 0)
                    {
                        string flagToMethod = flag.ToJavaMethod();

                        if (string.IsNullOrEmpty(flagToMethod))
                        {
                            continue;
                        }

                        _contentBuilder.Append(flagToMethod).Append(' ');
                    }
                }

                var methodWriter = ParserUtils.MethodDescriptorToJava(method);

                AppendObjectWithPackage(methodWriter.Type);

                _contentBuilder.Append(string.Concat(Enumerable.Repeat("[]", methodWriter.ArrayDepth)));

                _contentBuilder.Append(' ').Append(method.Name is ConstructorSpecialMethodName
                    ? _javaClass.ClassFileName
                    : method.Name);

                _contentBuilder.Append('(');

                for (int i = 0; i < methodWriter.Arguments.Count; i++)
                {
                    AppendObjectWithPackage(methodWriter.Arguments[i].Name);
                    _contentBuilder.Append(string.Concat(Enumerable.Repeat("[]", methodWriter.Arguments[i].ArrayDepth)))
                        .Append(' ')
                        .Append($"arg{i}").Append(',');
                }

                if (methodWriter.Arguments.Any())
                {
                    _contentBuilder.Length -= 1;
                }

                _contentBuilder.Append(')');

                if (_javaClass.IsInterface())
                {
                    _contentBuilder.Append(';');
                }
                else
                {
                    _contentBuilder.Append("{\n\t\t}");
                }

                _contentBuilder.Append("\n\n");
            }
        }

        private void AppendObjectWithPackage(string objectDescription, string originalObject = "")
        {
            var description = string.IsNullOrEmpty(objectDescription) ? originalObject : objectDescription;

            if (description.StartsWith('L'))
            {
                if (!description.StartsWith(JavaLangPackageDefinition))
                {
                    string import = $"import {description[1..].Replace("/", ".")}";

                    if (!_importsList.Contains(import))
                    {
                        _importsList.Add(import);
                    }
                }

                _contentBuilder.Append(description[(description.LastIndexOf('/') + 1)..].Replace(";", string.Empty));
            }
            else
            {
                _contentBuilder.Append(description);
            }
        }
    }
}