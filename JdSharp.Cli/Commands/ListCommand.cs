using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JdSharp.Cli.Handlers;
using System.Threading.Tasks;
using JdSharp.Cli.Interfaces;

namespace JdSharp.Cli.Commands;

[Command("list")]
public class ListCommand : ICommand
{
    private readonly IHandler<ListCommand> _handler;

    public ListCommand()
    {
        _handler = new ListHandler();
    }

    public async ValueTask ExecuteAsync(IConsole console) =>
        await _handler.Handle(this, console);
}