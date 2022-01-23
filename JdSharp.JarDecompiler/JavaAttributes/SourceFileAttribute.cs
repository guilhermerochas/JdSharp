namespace JdSharp.JarDecompiler.JavaAttributes
{
    public class SourceFileAttribute : BaseAttribute
    {
        public string SourceFile { get; }

        public SourceFileAttribute(string sourceFile)
        {
            SourceFile = sourceFile;
        }
    }
}