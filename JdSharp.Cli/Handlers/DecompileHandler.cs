using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CliFx.Infrastructure;
using JdSharp.Core.Models;
using JdSharp.Core.Utils;

namespace JdSharp.Cli.Handlers
{
    public class DecompileHandler
    {
        public async Task HandleDecompilerAsync(IConsole console, string inputFile, string outputFile, string outputDir)
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

            await console.Output.WriteLineAsync($"found {inputFile}");
            await console.Output.WriteLineAsync($"decompiling...");

            try
            {
                var decompilerResult = decompiler.Decompile(new DecompilerOptions
                {
                    Console = console.Output,
                    FileSignature = signature,
                    InputFileName = inputFile
                });

                if (string.IsNullOrEmpty(outputDir))
                {
                    outputDir = Environment.CurrentDirectory;
                }

                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                if (decompilerResult.FileContents.Count == 1)
                {
                    var fileNameBuilder = new StringBuilder();

                    if (string.IsNullOrEmpty(inputFile))
                    {
                        fileNameBuilder.Append(
                            decompilerResult.FileName.Remove(
                                decompilerResult.FileName.LastIndexOf(".", StringComparison.Ordinal)));
                    }
                    else
                    {
                        fileNameBuilder.Append(inputFile);
                    }

                    fileNameBuilder.Append('.').Append(decompiler.FileExtension());

                    await using StreamWriter fileWriter =
                        new StreamWriter(Path.Combine(outputDir, fileNameBuilder.ToString()), false, Encoding.ASCII);

                    await fileWriter.BaseStream.WriteAsync(decompilerResult.FileContents[0].Data, 0,
                        decompilerResult.FileContents[0].Data.Length);

                    return;
                }
                else
                {
                    await console.Output.WriteLineAsync($"Found {decompilerResult.FileContents.Count} inner files");

                    if (outputDir == Environment.CurrentDirectory)
                    {
                        outputDir = Directory.CreateDirectory(inputFile).FullName;
                    }

                    foreach (var fileContent in decompilerResult.FileContents)
                    {
                        var fileName =
                            fileContent.Path.Remove(fileContent.Path.LastIndexOf(".", StringComparison.Ordinal)) + '.' +
                            decompiler.FileExtension();

                        await using StreamWriter fileWriter =
                            new StreamWriter(Path.Combine(outputDir, fileName), false, Encoding.ASCII);

                        await fileWriter.BaseStream.WriteAsync(fileContent.Data, 0, fileContent.Data.Length);
                    }
                }

                await console.Output.WriteLineAsync("File decompiled successfully");
            }
            catch (Exception exception)
            {
                await console.Error.WriteLineAsync($"Erro: {exception.Message}");
            }
        }
    }
}