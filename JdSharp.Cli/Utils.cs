using System;
using System.IO;
using System.Linq;
using System.Text;
using JdSharp.Core;
using JdSharp.JarDecompiler;

namespace JdSharp.Cli
{
    public static class Utils
    {
        public static void GetDecompilerTypeFromHex(string filePath)
        {
            if (!File.Exists(filePath) || string.IsNullOrEmpty(filePath))
                throw new Exception("file not found!");
            
            using var binaryReader = new EndianessBinaryReader(File.OpenRead(filePath));
            JavaClassFile classFile = new JavaClassFile(binaryReader);
            Console.Out.WriteLine(classFile);
        }
    }
}