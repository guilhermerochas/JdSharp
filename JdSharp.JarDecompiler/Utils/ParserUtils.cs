using System;
using JdSharp.JarDecompiler.BufferWriters;
using JdSharp.JarDecompiler.ClassFileProperties;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JdSharp.JarDecompiler.Utils
{
    public static class ParserUtils
    {
        public static string FieldDescriptorToJava(string descriptor)
        {
            return descriptor switch
            {
                "D" => "double",
                "B" => "byte",
                "F" => "float",
                "I" => "int",
                "S" => "short",
                "Z" => "Boolean",
                "C" => "char",
                "V" => "void",
                _ => string.Empty
            };
        }

        public static MethodWriter MethodDescriptorToJava(Method method)
        {
            var methodWriter = new MethodWriter
            {
                Arguments = new List<Argument>
                {
                    new()
                }
            };

            ReadOnlySpan<char> descriptor = method.Descriptor.AsSpan();
            int descriptorIndex = 0;
            int argumentIndex = 0;

            if (descriptor[descriptorIndex] is '(')
            {
                bool isDescriptor = true;
                descriptorIndex++;
                var typeStringBuilder = new StringBuilder();

                while (descriptor[descriptorIndex] is not ')')
                {
                    switch (descriptor[descriptorIndex])
                    {
                        case '[':
                            methodWriter.Arguments[argumentIndex].ArrayDepth += 1;
                            break;
                        case ';':
                        {
                            string? type = typeStringBuilder.ToString();
                            string? argumentType = FieldDescriptorToJava(type);

                            methodWriter.Arguments[argumentIndex].Name =
                                string.IsNullOrEmpty(argumentType) ? type : argumentType;

                            methodWriter.Arguments.Add(new Argument());
                            typeStringBuilder.Length = 0;
                            argumentIndex++;
                            isDescriptor = true;

                            break;
                        }
                        case 'L':
                            isDescriptor = false;
                            typeStringBuilder.Append(descriptor[descriptorIndex]);
                            break;
                        default:
                        {
                            if (!isDescriptor)
                            {
                                typeStringBuilder.Append(descriptor[descriptorIndex]);
                                break;
                            }

                            methodWriter.Arguments[argumentIndex].Name =
                                FieldDescriptorToJava(descriptor[descriptorIndex].ToString());

                            methodWriter.Arguments.Add(new Argument());
                            argumentIndex++;
                            break;
                        }
                    }

                    descriptorIndex++;
                }


                descriptorIndex++;

                methodWriter.Arguments.RemoveAt(argumentIndex);

                while (descriptor[descriptorIndex].Equals('['))
                {
                    methodWriter.ArrayDepth += 1;
                    descriptorIndex += 1;
                }

                string? methodType = FieldDescriptorToJava(descriptor[descriptorIndex..].ToString());
                
                methodWriter.Type = string.IsNullOrEmpty(methodType)
                    ? descriptor[descriptorIndex..].ToString()
                    : methodType;
            }

            return methodWriter;
        }
    }
}