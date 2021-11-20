namespace JdSharp.Core
{
    public interface IBaseDecompiler 
    {
        public string FileName { get; set; }
        public string OutFileName { get; set; }
        public bool Decompile();
    }
}