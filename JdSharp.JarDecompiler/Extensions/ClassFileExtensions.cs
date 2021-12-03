using System;
using JdSharp.JarDecompiler.Constants;

namespace JdSharp.JarDecompiler.Extensions
{
    public static class ClassFileExtensions
    {
        public static string GetConstantFromUshort(this BaseConstant[] constants, ushort value)
        {
            ushort nameIndex = ((ClassConstant)constants[value - 1]).NameIndex;
            string? valueFromUshort = ((Utf8Constant)constants[nameIndex - 1]).Value;
            
            if (!string.IsNullOrEmpty(valueFromUshort))
                return valueFromUshort;
            
            throw new Exception($"Not able to get Utf8 from index at {value - 1} in constants");
        }
        
        public static string GetUft8ConstantFromUshort(this BaseConstant[] constants, ushort value)
        {
            string? stringFromUtf8Constant = ((Utf8Constant)constants[value - 1])?.Value;

            if (!string.IsNullOrEmpty(stringFromUtf8Constant))
                return stringFromUtf8Constant;
            
            throw new Exception($"Not able to get Utf8 from index at {value - 1} in constants");
        }
    }
}