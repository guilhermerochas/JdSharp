using System.Threading.Tasks;
using CliFx;

namespace JdSharp.Cli
{
    internal static class Program
    {
        public static async Task<int> Main(string[] args) =>
            await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .Build()
                .RunAsync(new [] {"decompile", @"C:\Users\ThinkPad\Desktop\DotNet\JdSharp\JdSharp.Cli\bin\Debug\net5.0\VarState.class"});
    }
}