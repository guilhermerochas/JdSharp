using System.IO;

namespace JdSharp.Core.Models;

public class DecompilerOptions
{
    public StreamWriter Console { get; set; }
    public byte[] FileSignature { get; set; }
    public string InputFileName { get; set; }
}