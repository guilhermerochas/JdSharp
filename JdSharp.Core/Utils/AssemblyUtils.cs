#nullable enable

using JdSharp.Core.Decompilers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace JdSharp.Core.Utils;

public static class AssemblyUtils
{
    public static (IDecompiler?, byte[]? fileSignature) GetDecompilerFromFile(string inputFile,
        IEnumerable<Assembly>? assemblies = null)
    {
        Type? decompilerType = typeof(IDecompiler);

        assemblies ??= GetAssemblyWithReferences();

        List<Type>? implementations = assemblies.SelectMany(assembly => assembly.GetTypes())
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

        foreach (IDecompiler? instance in decompilersInstances)
        {
            if (instance is null)
            {
                continue;
            }

            foreach (byte[]? allowedFileSign in instance.AllowedFileSignatures)
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
        List<Assembly>? loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        string[]? loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
        string[]? referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
        List<string>? toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase))
            .ToList();
        toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

        return loadedAssemblies;
    }
}