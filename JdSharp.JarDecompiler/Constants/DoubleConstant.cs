using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class DoubleConstant : BaseConstant
    {
        public double Value { get; }

        public DoubleConstant(double value) : base((uint)ConstantEnum.Double)
        {
            Value = value;
        }
    }
}