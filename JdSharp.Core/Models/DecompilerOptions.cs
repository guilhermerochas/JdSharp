using System.IO;

namespace JdSharp.Core.Models;

public class DecompilerOptions
{
    public TextWriter Console { get; set; }
    public byte[] FileSignature { get; set; }
    public string InputFileName { get; set; }
}