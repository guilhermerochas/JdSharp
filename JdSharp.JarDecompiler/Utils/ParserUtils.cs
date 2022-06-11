using JdSharp.JarDecompiler.BufferWriters;
using JdSharp.JarDecompiler.ClassFileProperties;
using System.Collections.Generic;
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
            MethodWriter? methodWriter = new MethodWriter
            {
                Arguments = new List<Argument>
                {
                    new()
                }
            };
            string? descriptor = method.Descriptor;

            int descriptorIndex = 0;
            int argumentIndex = 0;

            if (descriptor[descriptorIndex].Equals('('))
            {
                StringBuilder typeBuilder = new();
                descriptorIndex++;
                while (!descriptor[descriptorIndex].Equals(')'))
                {
                    switch (descriptor[descriptorIndex])
                    {
                        case '[':
                            methodWriter.Arguments[argumentIndex].ArrayDepth += 1;
                            break;
                        case ';':
                            {
                                string? type = typeBuilder.ToString();

                                string? argumentType = FieldDescriptorToJava(type);

                                methodWriter.Arguments[argumentIndex].Name = string.IsNullOrEmpty(argumentType) ? type : argumentType;

                                methodWriter.Arguments.Add(new Argument());
                                argumentIndex += 1;
                                break;
                            }
                        default:
                            typeBuilder.Append(descriptor[descriptorIndex]);
                            break;
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

                string? methodType = FieldDescriptorToJava(descriptor[descriptorIndex..]);
                methodWriter.Type = string.IsNullOrEmpty(methodType) ? descriptor[descriptorIndex..] : methodType;
            }

            return methodWriter;
        }
    }
}