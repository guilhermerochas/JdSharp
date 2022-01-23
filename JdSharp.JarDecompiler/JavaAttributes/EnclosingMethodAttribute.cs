using JdSharp.JarDecompiler.Constants;

namespace JdSharp.JarDecompiler.JavaAttributes
{
    public class EnclosingMethodAttribute : BaseAttribute
    {
        public string Class { get; }
        
        public NameAndTypeConstant Method { get; }

        public EnclosingMethodAttribute(string classValue, NameAndTypeConstant method)
        {
            Class = classValue;
            Method = method;
        }
    }
}