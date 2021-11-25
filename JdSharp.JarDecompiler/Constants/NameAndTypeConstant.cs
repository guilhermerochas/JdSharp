using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class NameAndTypeConstant : BaseConstant
    {
        public ushort NameIndex { get; }
        public ushort DescriptorIndex { get; }

        public NameAndTypeConstant(ushort nameIndex, ushort descriptorIndex) : base((uint)ConstantEnum.NameAndType)
        {
            NameIndex = nameIndex;
            DescriptorIndex = descriptorIndex;
        }
    }
}