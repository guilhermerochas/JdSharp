namespace JdSharp.JarDecompiler.Constants
{
    public class BaseConstant
    {
        public uint Tag { get; protected set; }

        public BaseConstant(uint tag)
            => Tag = tag;
    }
}