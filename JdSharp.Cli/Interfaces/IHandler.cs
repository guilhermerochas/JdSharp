using System.Threading.Tasks;
using CliFx.Infrastructure;
using ICommand = CliFx.ICommand;

namespace JdSharp.Cli.Interfaces;

public interface IHandler<in TCommand> where TCommand : ICommand
{
    public Task Handle(TCommand command, IConsole console);
}