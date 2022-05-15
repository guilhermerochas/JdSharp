using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JdSharp.Cli.Handlers;

namespace JdSharp.Cli.Commands
{
    [Command("decompile")]
    public class DecompileCommand : ICommand
    {
        [CommandParameter(0, Description = "Input file path containing file to decompile")]
        public string InputFilePath { get; init; } = string.Empty;

        [CommandOption("output", 'o', Description = "Output file of the generated content")]
        public string OututFileName { get; init; } = string.Empty;

        [CommandOption("directory", 'd', Description = "Output folder of the generated content")]
        public string OututDirectory { get; init; } = string.Empty;

        private readonly DecompileHandler _handler = new();

        public async ValueTask ExecuteAsync(IConsole console)
            => await _handler.HandleDecompilerAsync(console, InputFilePath, OututFileName, OututDirectory);
    }
}