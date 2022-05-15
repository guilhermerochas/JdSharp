using System;
using System.IO;
using System.Threading.Tasks;
using CliFx.Infrastructure;
using JdSharp.Core.Decompilers;
using JdSharp.Core.Utils;

namespace JdSharp.Cli.Handlers
{
    public class DecompileHandler
    {
        private static bool IsCurrentDirectory(string path) => path is "."; 
        
        public async Task HandleDecompilerAsync(IConsole console, string inputFile, string outputDirectory)
        {
            if (!File.Exists(inputFile))
            {
                await console.Error.WriteLineAsync($"file {inputFile} not found");
                return;
            }

            IDecompiler? decompiler = AssemblyUtils.GetDecompilerFromFile(inputFile);

            if (decompiler is null)
            {
                await console.Error.WriteLineAsync("decompiler plug-in for this file was not found!");
                return;
            }
            
            if (IsCurrentDirectory(outputDirectory))
            {
                outputDirectory = Environment.CurrentDirectory;
            }

            await console.Output.WriteLineAsync($"{inputFile} in {outputDirectory}");
        }
    }
}