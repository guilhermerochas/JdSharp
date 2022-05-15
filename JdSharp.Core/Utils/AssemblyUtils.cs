#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JdSharp.Core.Decompilers;

namespace JdSharp.Core.Utils;

public static class AssemblyUtils
{
    public static IDecompiler? GetDecompilerFromFile(string inputFile)
    {
        var decompilerType = typeof(IDecompiler);

        var implementations = GetAssemblyWithReferences().SelectMany(assembly => assembly.GetTypes())
            .Where(type => decompilerType.IsAssignableFrom(type) && !type.IsInterface).ToList();

        IEnumerable<IDecompiler?> decompilersInstances = implementations.Select(implementation => Activator.CreateInstance(implementation) as IDecompiler);

        foreach (var instance in decompilersInstances)
        {
            if (instance is null)
            {
                continue;
            }

            foreach (var allowedFileSign in instance.AllowedFileSignatures)
            {
                if (allowedFileSign[0] is 0xca)
                {
                    return instance;
                }
            }
        }

        return null;
    }
    
    public static IEnumerable<Assembly> GetAssemblyWithReferences()
    {
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
        var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
        var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
        toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

        return loadedAssemblies;
    }
}