using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class MethodTypeConstant : BaseConstant
    {
        public ushort DescriptorIndex { get; }

        public MethodTypeConstant(ushort descriptorIndex) : base((uint)ConstantEnums.MethodType)
        {
            DescriptorIndex = descriptorIndex;
        }
    }
}