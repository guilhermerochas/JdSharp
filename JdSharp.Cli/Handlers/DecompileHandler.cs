using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliFx.Infrastructure;
using JdSharp.Core.Decompilers;
using JdSharp.Core.Models;
using JdSharp.Core.Utils;

namespace JdSharp.Cli.Handlers
{
    public class DecompileHandler
    {
        public async Task HandleDecompilerAsync(IConsole console, string inputFile, string outputDirectory)
        {
            if (!File.Exists(inputFile))
            {
                await console.Error.WriteLineAsync($"file {inputFile} not found");
                return;
            }

            var (decompiler, signature) = AssemblyUtils.GetDecompilerFromFile(inputFile);

            if (decompiler is null)
            {
                await console.Error.WriteLineAsync("decompiler plug-in for this file was not found!");
                return;
            }

            await console.Output.WriteLineAsync($"found {inputFile} in {outputDirectory}");

            var decompilerResult = decompiler.Decompile(new DecompilerOptions
            {
                Console = console.Output,
                FileSignature = signature,
                InputFileName = inputFile
            });

            if (decompilerResult.FileContents.Count == 1)
            {
                if (string.IsNullOrEmpty(outputDirectory))
                {
                    string fileName =
                        decompilerResult.FileName.Remove(
                            decompilerResult.FileName.LastIndexOf(".", StringComparison.Ordinal)) + '.' +
                        decompiler.FileExtension();

                    await using StreamWriter fileWriter =
                        new StreamWriter(fileName, false, Encoding.ASCII);

                    await fileWriter.BaseStream.WriteAsync(decompilerResult.FileContents[0].Data, 0,
                        decompilerResult.FileContents[0].Data.Length);
                }

                return;
            }

            await console.Output.WriteLineAsync("Multiple Files");
        }
    }
}