using CliFx.Infrastructure;
using JdSharp.Core.Decompilers;
using JdSharp.Core.Utils;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JdSharp.Cli.Commands;
using JdSharp.Cli.Interfaces;

namespace JdSharp.Cli.Handlers;

public class ListHandler : IHandler<ListCommand>
{
    private readonly Type _decompilerType = typeof(IDecompiler);

    public async Task Handle(ListCommand command, IConsole console)
    {
        await console.Output.WriteLineAsync("Plugins: ");

        foreach (var decompilerType in AssemblyUtils.GetAssemblyWithReferences().SelectMany(asm => asm.GetTypes())
                     .Where(type => _decompilerType.IsAssignableFrom(type) && !type.IsInterface))
        {
            await console.Output.WriteLineAsync($" - {decompilerType.Name}");
        }
    }
}