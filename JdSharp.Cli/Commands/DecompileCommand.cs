using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JdSharp.Cli.Handlers;
using System.Threading.Tasks;
using JdSharp.Cli.Interfaces;

namespace JdSharp.Cli.Commands
{
    [Command("decompile")]
    public class DecompileCommand : ICommand
    {
        private readonly IHandler<DecompileCommand> _handler;
        
        [CommandParameter(0, Description = "Input file path containing file to decompile")]
        public string InputFilePath { get; init; } = string.Empty;

        [CommandOption("output", 'o', Description = "Output file of the generated content")]
        public string OututFileName { get; init; } = string.Empty;

        [CommandOption("directory", 'd', Description = "Output folder of the generated content")]
        public string OututDirectory { get; set; } = string.Empty;

        public DecompileCommand()
        {
            _handler = new DecompileHandler();
        }

        public async ValueTask ExecuteAsync(IConsole console)
            => await _handler.Handle(this, console);
    }
}