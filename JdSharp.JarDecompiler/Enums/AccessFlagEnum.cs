namespace JdSharp.JarDecompiler.Enums
{
    public enum AccessFlagEnum: ushort
    {
        AccPublic = 0x0001,
        AccFinal = 0x0010,
        AccSuper = 0x0020,
        AccInterface = 0x0200,
        AccAbstract = 0x0400,
        AccSynthetic = 0x1000, 
        AccAnnotation = 0x2000,
        AccEnum = 0x4000
    }
}