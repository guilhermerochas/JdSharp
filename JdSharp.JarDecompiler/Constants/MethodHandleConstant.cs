using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class MethodHandleConstant : BaseConstant
    {
        public byte ReferenceKind { get; }
        public ushort ReferenceIndex { get; }

        public MethodHandleConstant(byte referenceKind, ushort referenceIndex) : base((uint)ConstantEnum.MethodHandle)
        {
            ReferenceKind = referenceKind;
            ReferenceIndex = referenceIndex;
        }
    }
}