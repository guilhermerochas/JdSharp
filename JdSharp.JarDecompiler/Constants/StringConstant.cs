using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class StringConstant : BaseConstant
    {
        public ushort StringIndex { get; }

        public StringConstant(ushort stringIndex) : base((uint)ConstantEnums.String)
        {
            StringIndex = stringIndex;
        }
    }
}