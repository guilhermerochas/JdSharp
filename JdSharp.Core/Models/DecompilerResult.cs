using System.Collections.Generic;

namespace JdSharp.Core.Models;

public class DecompilerResult
{
    public string FileName { get; set; }
    public List<FileResult> FileContents { get; set; }
}