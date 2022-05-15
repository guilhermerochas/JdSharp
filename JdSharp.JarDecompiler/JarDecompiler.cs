using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using JdSharp.Core;
using JdSharp.Core.Decompilers;
using JdSharp.Core.Extensions;
using JdSharp.Core.Models;
using JdSharp.JarDecompiler.BufferWriters;

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
                using StreamReader fileReader = new StreamReader(options.InputFileName);
                using BinaryReader binaryReader = new BigEndianessBinaryReader(fileReader.BaseStream);
                
                JavaClassFile javaClassFile = JavaClassFile.FromBinaryStream(binaryReader);

                var buffer = new JavaClassWriter().Write(javaClassFile);
                fileResults.Add(new FileResult
                {
                    Path = options.InputFileName,
                    Data = buffer
                });
            }
            else
            {
                using StreamReader zipReader = new StreamReader(options.InputFileName);
                using var unziper = new ZipArchive(zipReader.BaseStream, ZipArchiveMode.Read);

                foreach (var entry in unziper.Entries.Where(entry => !entry.IsFolder()))
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    options.Console.WriteLine(entry.Name);
                    entry.Open().CopyTo(memoryStream);

                    using BinaryReader binaryReader = new BigEndianessBinaryReader(memoryStream);
                    binaryReader.BaseStream.Position = 0x00;
                    var fileSignature = binaryReader.ReadBytes(4);

                    if (!IsJavaClassFile(fileSignature))
                    {
                        continue;
                    }

                    JavaClassFile javaClassFile = JavaClassFile.FromBinaryStream(binaryReader);

                    var buffer = new JavaClassWriter().Write(javaClassFile);
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
                FileName = Path.GetFileName(options.InputFileName)
            };
        }
    }
}