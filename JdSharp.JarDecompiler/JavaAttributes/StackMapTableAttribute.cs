namespace JdSharp.JarDecompiler.JavaAttributes
{
    public class StackMapTableAttribute : BaseAttribute
    {
        public byte[] NumberOfEntries { get; }

        public StackMapTableAttribute(byte[] numberOfEntries)
        {
            NumberOfEntries = numberOfEntries;
        }
    }
}