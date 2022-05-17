using JdSharp.Core.Decompilers;

namespace JdSharp.Web.Services.Decompiler;

public class DecompilerService : IDecompilerService
{
    public IEnumerable<IDecompiler> GetDecompilers()
        => new List<IDecompiler>
        {
            new JarDecompiler.JarDecompiler()
        };
}