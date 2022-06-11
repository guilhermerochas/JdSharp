using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JdSharp.Cli.Handlers;
using System.Threading.Tasks;

namespace JdSharp.Cli.Commands;

[Command("list")]
public class ListCommand : ICommand
{
    private readonly ListHandler _listHandler = new();

    public async ValueTask ExecuteAsync(IConsole console) => await _listHandler.GetList(console);
}