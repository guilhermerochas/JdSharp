using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using JdSharp.Core;

namespace JdSharp.Cli
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            Utils.GetDecompilerTypeFromHex("./Hello.class");
        }
    }
}
