using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class DoubleConstant : BaseConstant
    {
        public double Value { get; }

        public DoubleConstant(double value) : base((uint)ConstantEnums.Double)
        {
            Value = value;
        }
    }
}