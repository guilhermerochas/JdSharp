using System.IO;

namespace JdSharp.JarDecompiler.JavaAttributes
{
    public class BaseAttribute
    {
        protected BaseAttribute()
        {
        }

        public BaseAttribute(ref BinaryReader reader, uint size)
        {
            reader.ReadBytes((int)size);
        }
    }
}