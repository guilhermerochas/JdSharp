using System.Threading.Tasks;

namespace JdSharp.Cli
{
    internal static class Program
    {
        private static int Main(string[] args) =>
            Utils.GetDecompilerTypeFromHex("./SwitchTeste.class");
    }
}