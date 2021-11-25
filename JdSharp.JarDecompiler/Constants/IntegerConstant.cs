using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class IntegerConstant : BaseConstant
    {
        public uint Value { get; }

        public IntegerConstant(uint value) : base((uint)ConstantEnums.Integer)
            => Value = value;
    }
}