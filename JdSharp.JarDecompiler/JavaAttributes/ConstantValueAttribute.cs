namespace JdSharp.JarDecompiler.JavaAttributes
{
    public class ConstantValueAttribute : BaseAttribute
    {
        public string ConstantValue { get; }

        public ConstantValueAttribute(string constantValue)
        {
            ConstantValue = constantValue;
        }
    }
}