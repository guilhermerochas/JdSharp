using CliFx.Infrastructure;
using JdSharp.Core.Models;
using JdSharp.Core.Utils;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JdSharp.Cli.Commands;
using JdSharp.Cli.Interfaces;

namespace JdSharp.Cli.Handlers
{
    public class DecompileHandler : IHandler<DecompileCommand>
    {
        public async Task Handle(DecompileCommand command, IConsole console)
        {
            if (!File.Exists(command.InputFilePath))
            {
                await console.Error.WriteLineAsync($"file {command.InputFilePath} not found");
                return;
            }

            var (decompiler, signature) = AssemblyUtils.GetDecompilerFromFile(command.InputFilePath);

            if (decompiler is null)
            {
                await console.Error.WriteLineAsync("decompiler plug-in for this file was not found!");
                return;
            }

            await console.Output.WriteLineAsync($"found {command.InputFilePath}");
            await console.Output.WriteLineAsync($"decompiling...");

            try
            {
                using StreamReader streamReader = new StreamReader(command.InputFilePath);
                DecompilerResult? decompilerResult = decompiler.Decompile(new DecompilerOptions
                {
                    Console = console.Output,
                    FileSignature = signature,
                    InputFileStream = streamReader.BaseStream,
                    FileName = command.InputFilePath
                });

                if (string.IsNullOrEmpty(command.OututDirectory))
                {
                    if (decompilerResult.FileContents.Count is 1)
                    {
                        command.OututDirectory = Environment.CurrentDirectory;
                    }
                    else
                    {
                        command.OututDirectory = Path.Combine(Environment.CurrentDirectory,
                            Path.GetFileNameWithoutExtension(command.InputFilePath));
                    }
                }

                if (!Directory.Exists(command.OututDirectory))
                {
                    Directory.CreateDirectory(command.OututDirectory);
                }

                if (decompilerResult.FileContents.Count is 1)
                {
                    StringBuilder fileNameBuilder = new StringBuilder();

                    if (!Directory.Exists(command.OututDirectory))
                    {
                        Directory.CreateDirectory(command.OututDirectory);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(string.IsNullOrEmpty(command.OututFileName)
                        ? command.InputFilePath
                        : command.OututFileName) + "." + decompiler.FileExtension();

                    fileNameBuilder.Append(Path.Combine(command.OututDirectory, fileName));

                    await using StreamWriter fileWriter =
                        new StreamWriter(fileNameBuilder.ToString(), false,
                            Encoding.ASCII);

                    await fileWriter.BaseStream.WriteAsync(decompilerResult.FileContents.First().Data, 0,
                        decompilerResult.FileContents.First().Data.Length);

                    return;
                }

                await console.Output.WriteLineAsync($"Found {decompilerResult.FileContents.Count} inner files");

                if (command.OututDirectory == Environment.CurrentDirectory)
                {
                    command.OututDirectory = Directory.CreateDirectory(command.InputFilePath).FullName;
                }

                foreach (FileResult? fileContent in decompilerResult.FileContents)
                {
                    string directoryName = Path.Combine(command.OututDirectory,
                        Path.GetDirectoryName(fileContent.Path)!);

                    string fileName = Path.GetFileNameWithoutExtension(fileContent.Path) + '.' +
                                      decompiler.FileExtension();

                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    await using StreamWriter fileWriter =
                        new StreamWriter(Path.Combine(directoryName, fileName), false, Encoding.ASCII);

                    await fileWriter.BaseStream.WriteAsync(fileContent.Data, 0, fileContent.Data.Length);
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