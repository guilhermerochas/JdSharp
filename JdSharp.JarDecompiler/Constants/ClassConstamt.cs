using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class ClassConstant : BaseConstant
    {
        public ushort NameIndex { get; }

        public ClassConstant(ushort nameIndex) : base((uint)ConstantEnum.Class)
            => NameIndex = nameIndex;
    }
}