using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class FloatConstant : BaseConstant
    {
        public float Value { get; }

        public FloatConstant(float value) : base((uint)ConstantEnum.Float)
            => Value = value;
    }
}