using CommandLine;
// nuget CommandLineParser https://github.com/commandlineparser/commandline

namespace ImageTool
{
    class Options
    {

        // Mandatory parameters

        [Option('s', "SourceDir", Required = true, HelpText = "Directory with input files to be processed.")]
        public string SourceDir { get; set; }

        [Option('m', "FileInputMask", Required = true, HelpText = "Mask input files to be processed. E.g.: *.tif")]
        public string FileInputMask { get; set; }

        [Option('t', "TargetDir", Required = true, HelpText = "Directory for output.")]
        public string TargetDir { get; set; }

        [Option('o', "OutFormat", Required = true, HelpText = "Output format: [png | gif | bmp | tif | jpg]")]
        public string OutFormat { get; set; }

        // Optional parameters

        [Option('w', "OverWrite", Required = false, Default = false, HelpText = "OverWriteFlag: [true | false]")]
        public bool OverWrite { get; set; }

        [Option('k', "Watermark", Required = false, Default = "", HelpText = "Watermark PNG file")]
        public string WatermarkFile { get; set; }

        [Option('p', "WatermarkOpacity", Required = false, Default = 100, HelpText = "Watermark opacity (0-100)")]
        public int WatermarkOpacity { get; set; }

        [Option('x', "MaxWidth", Required = false, Default = 0, HelpText = "Target max Width")]
        public int MaxWidth { get; set; }

        [Option('y', "MaxHeight", Required = false, Default = 0, HelpText = "Target max Height")]
        public int MaxHeight { get; set; }

    }
}
