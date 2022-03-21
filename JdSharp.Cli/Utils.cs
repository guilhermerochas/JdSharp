using System;
using System.IO;
using JdSharp.Core;
using JdSharp.JarDecompiler;

namespace JdSharp.Cli
{
    public static class Utils
    {
        public static int GetDecompilerTypeFromHex(string filePath)
        {
            if (!File.Exists(filePath) || string.IsNullOrEmpty(filePath))
            {
                Console.Error.WriteLine("Arquivo não encontrado!");
                return 1;
            }
            
            using BinaryReader binaryReader = new BigEndianessBinaryReader(File.OpenRead(filePath));
            JavaClassFile classFile = JavaClassFile.FromBinaryStream(binaryReader);
            Console.Out.WriteLine(classFile);

            return 0;
        }
    }
}