using System;

namespace JdSharp.JarDecompiler.Enums
{
    [Flags]
    public enum AccessFlagEnum: ushort
    {
        AccPublic = 0x0001,
        AccPrivate = 0x0002,
        AccProtected = 0x0004,
        AccStatic = 0x0008,
        AccFinal = 0x0010,
        AccSuper = 0x0020,
        AccBridge = 0x0040,
        AccVarargs = 0x0080,
        AccNative = 0x0100,
        AccInterface = 0x0200,
        AccAbstract = 0x0400,
        AccStrict = 0x0800,
        AccSynthetic = 0x1000, 
        AccAnnotation = 0x2000,
        AccEnum = 0x4000
    }
}