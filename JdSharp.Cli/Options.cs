using CommandLine;

namespace JdSharp.Cli
{
    public class Options
    {
        [Option('f', "filename", Required = true, HelpText = "name of input file to be decompiled")]
        public string FileName { get; set; } = string.Empty;

        [Option('o', "output", Required = false,
            HelpText = "name of output file the name of folder/file of the output result")]
        public string? OutputFile { get; set; } = "decompiled";
    }
}