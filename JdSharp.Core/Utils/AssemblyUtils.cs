#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JdSharp.Core.Decompilers;

namespace JdSharp.Core.Utils;

public static class AssemblyUtils
{
    public static (IDecompiler?, byte[]? fileSignature) GetDecompilerFromFile(string inputFile,
        IEnumerable<Assembly>? assemblies = null)
    {
        var decompilerType = typeof(IDecompiler);

        assemblies ??= GetAssemblyWithReferences();

        var implementations = assemblies.SelectMany(assembly => assembly.GetTypes())
            .Where(type => decompilerType.IsAssignableFrom(type) && !type.IsInterface).ToList();

        IEnumerable<IDecompiler?> decompilersInstances =
            implementations.Select(implementation => Activator.CreateInstance(implementation) as IDecompiler);

        using StreamReader streamReader = new StreamReader(inputFile);
        
        return GetDecompilerFromStream(streamReader.BaseStream, decompilersInstances);
    }

    public static (IDecompiler?, byte[]? fileSignature) GetDecompilerFromStream(Stream fileInputStream, IEnumerable<IDecompiler?> decompilersInstances)
    {
        byte[] fileSignatue;
        
        using (BinaryReader binaryReader = new BigEndianessBinaryReader(fileInputStream))
        {
            fileSignatue = binaryReader.ReadBytes(4);
        }

        foreach (var instance in decompilersInstances)
        {
            if (instance is null)
            {
                continue;
            }
            
            foreach (var allowedFileSign in instance.AllowedFileSignatures)
            {
                if (fileSignatue.SequenceEqual(allowedFileSign))
                {
                    return (instance, fileSignatue);
                }
            }
        }

        return (null, null);
    }

    public static IEnumerable<Assembly> GetAssemblyWithReferences()
    {
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
        var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
        var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase))
            .ToList();
        toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

        return loadedAssemblies;
    }
}