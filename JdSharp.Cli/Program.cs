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
            return;
            
            /*await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async opts =>
            {
                try
                {
                    if (!File.Exists(opts.FileName))
                        throw new Exception($"{opts.FileName} wasn't able to be found on the provided path");
                    
                    var decompiler = Utils.GetDecompilerTypeFromHex(opts.FileName);
                    decompiler.FileName = opts.FileName;
                    decompiler.OutFileName = opts.OutputFile;
                    decompiler.Decompile();
                }
                catch (Exception e)
                {
                    await Console.Error.WriteLineAsync(e.Message);
                }
            });*/
        }
    }
}
