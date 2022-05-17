using System.IO;

namespace JdSharp.Core.Models;

public class DecompilerOptions
{
    public TextWriter Console { get; set; }
    public byte[] FileSignature { get; set; }
    public Stream InputFileStream { get; set; }
    public string FileName { get; set; }
}