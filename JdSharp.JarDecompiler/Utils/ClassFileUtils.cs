using System;
using System.IO;
using JdSharp.JarDecompiler.Constants;

namespace JdSharp.JarDecompiler.Utils
{
    public static class ClassFileUtils
    {
        public static BaseConstant GetConstantType(ushort constantTag, BinaryReader reader)
        {
            return constantTag switch
            {
                1 => new Utf8Constant(),
                3 => new IntegerConstant(),
                4 => new FloatConstant(),
                5 => new FloatConstant(),
                6 => new DoubleConstant(),
                7 | 19 | 20 => new ClassConstamt(),
                8 => new StringConstant(),
                9 | 10 | 11 | 17 | 18 => new MemberRefConstant(),
                12 => new NameAndTypeConstant(),
                15 => new MethodHandleConstant(),
                16 => new MethodTypeConstant(),
                _ => throw new Exception($"Not able to get constant with tag: {constantTag}")
            };
        }
    }
}