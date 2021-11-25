using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class IntegerConstant : BaseConstant
    {
        public uint Value { get; }

        public IntegerConstant(uint value) : base((uint)ConstantEnum.Integer)
            => Value = value;
    }
}