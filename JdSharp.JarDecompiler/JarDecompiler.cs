using JdSharp.Core;
using JdSharp.Core.Decompilers;
using JdSharp.Core.Extensions;
using JdSharp.Core.Models;
using JdSharp.JarDecompiler.BufferWriters;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace JdSharp.JarDecompiler
{
    public class JarDecompiler : IDecompiler
    {
        public IEnumerable<byte[]> AllowedFileSignatures { get; } = new List<byte[]>
        {
            new byte[] { 0xca, 0xfe, 0xba, 0xbe },
            new byte[] { 0x50, 0x4b, 0x03, 0x04 }
        };

        private bool IsJavaClassFile(byte[] fileSignature) =>
            fileSignature.SequenceEqual(AllowedFileSignatures.ElementAt(0));

        public string FileExtension() => "java";

        public DecompilerResult Decompile(DecompilerOptions options)
        {
            List<FileResult> fileResults = new List<FileResult>();

            if (IsJavaClassFile(options.FileSignature))
            {
                using BinaryReader binaryReader = new BigEndianessBinaryReader(options.InputFileStream);

                JavaClassFile javaClassFile = JavaClassFile.FromBinaryStream(binaryReader);

                byte[]? buffer = new JavaClassWriter().Write(javaClassFile);
                fileResults.Add(new FileResult
                {
                    Path = options.FileName,
                    Data = buffer
                });
            }
            else
            {
                using ZipArchive? unziper = new ZipArchive(options.InputFileStream, ZipArchiveMode.Read);

                foreach (ZipArchiveEntry? entry in unziper.Entries.Where(entry => !entry.IsFolder()))
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    options.Console.WriteLine(entry.Name);
                    entry.Open().CopyTo(memoryStream);

                    using BinaryReader binaryReader = new BigEndianessBinaryReader(memoryStream);
                    binaryReader.BaseStream.Position = 0x00;
                    byte[]? fileSignature = binaryReader.ReadBytes(4);

                    if (!IsJavaClassFile(fileSignature))
                    {
                        continue;
                    }

                    JavaClassFile javaClassFile = JavaClassFile.FromBinaryStream(binaryReader);

                    byte[]? buffer = new JavaClassWriter().Write(javaClassFile);
                    fileResults.Add(new FileResult
                    {
                        Path = entry.Name,
                        Data = buffer
                    });
                }
            }

            return new DecompilerResult
            {
                FileContents = fileResults,
                FileName = Path.GetFileName(options.FileName)
            };
        }
    }
}