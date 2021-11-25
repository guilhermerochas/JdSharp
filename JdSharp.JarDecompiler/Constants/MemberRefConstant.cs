using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class MemberRefConstant : BaseConstant
    {
        public ushort ClassIndex { get; }
        public ushort NameAndTypeIndex { get; }

        public MemberRefConstant(ushort classIndex, ushort nameAndTypeIndex) : base((uint)ConstantEnums.MemberRef)
        {
            ClassIndex = classIndex;
            NameAndTypeIndex = nameAndTypeIndex;
        }
    }
}