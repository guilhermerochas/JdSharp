using JdSharp.JarDecompiler.ClassFileProperties;
using JdSharp.JarDecompiler.Constants;
using System.IO;

namespace JdSharp.JarDecompiler.JavaAttributes
{
    public class InnerClassesAttribute : BaseAttribute
    {
        public Class[] Classes { get; }

        private InnerClassesAttribute(Class[] classes)
        {
            Classes = classes;
        }

        public static InnerClassesAttribute FromBinaryStream(ref BinaryReader reader, BaseConstant[] constants)
        {
            ushort numberOfClasses = reader.ReadUInt16();
            Class[] classes = new Class[numberOfClasses];

            for (int i = 0; i < numberOfClasses; i++)
            {
                classes[i] = Class.FromBinaryStream(ref reader, constants);
            }

            return new InnerClassesAttribute(classes);
        }
    }
}