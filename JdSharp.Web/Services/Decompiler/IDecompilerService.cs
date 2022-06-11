using JdSharp.Core.Decompilers;

namespace JdSharp.Web.Services.Decompiler;

public interface IDecompilerService
{
    public IEnumerable<IDecompiler> GetDecompilers();
}