using CliFx.Infrastructure;
using JdSharp.Core.Decompilers;
using JdSharp.Core.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JdSharp.Cli.Handlers;

public class ListHandler
{
    public async Task GetList(IConsole console)
    {
        await console.Output.WriteLineAsync("Plugins: ");

        foreach (Type decompiler in
            AssemblyUtils.GetAssemblyWithReferences().SelectMany(assemly => assemly.GetTypes())
            .Where(type => typeof(IDecompiler).IsAssignableFrom(type) && !type.IsInterface))
        {
            if (decompiler is not null)
            {
                await console.Output.WriteLineAsync($" - {decompiler.Name}");
            }
        }
    }
}