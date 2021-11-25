using System.IO;
using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Constants
{
    public class Utf8Constant : BaseConstant
    {
        public string Value { get; }

        public Utf8Constant(string value) : base((uint) ConstantEnums.Utf8)
        {
            Value = value;
        }
    }
}