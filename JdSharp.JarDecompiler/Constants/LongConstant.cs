using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class LongConstant : BaseConstant
    {
        public ulong Value { get; }
        
        public LongConstant(ulong value) : base((uint)ConstantEnums.Long)
        {
            Value = value;
        }
    }
}