using System.IO.Compression;

namespace JdSharp.Core.Extensions;

public static class ZipExtensions
{
    public static bool IsFolder(this ZipArchiveEntry entry) 
        => entry.FullName.EndsWith("/");
}