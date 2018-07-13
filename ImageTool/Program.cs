using System;
using System.Collections.Generic;
using System.Drawing; // reference System.Drawing
using System.Drawing.Imaging;
using System.IO;

using CommandLine;
// nuget CommandLineParser https://github.com/commandlineparser/commandline

namespace ImageTool
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            TimeSpan t0 = new TimeSpan(DateTime.UtcNow.Ticks);

            Parser.Default.ParseArguments<Options>(args)
                 .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
                 .WithNotParsed<Options>((errs) => HandleParseError(errs));

            TimeSpan t1 = new TimeSpan(DateTime.UtcNow.Ticks);

            Console.WriteLine();
            Console.WriteLine($"Duration: {(t1 - t0).ToString(@"hh\:mm\:ss")}");
        }

        private static int RunOptionsAndReturnExitCode(Options options)
        {
            try
            {
                ImageFormat outputFormat;
                switch (options.OutFormat.ToLower())
                {
                    case "png":
                        outputFormat = ImageFormat.Png;
                        break;
                    case "gif":
                        outputFormat = ImageFormat.Gif;
                        break;
                    case "bmp":
                        outputFormat = ImageFormat.Bmp;
                        break;
                    case "tif":
                        outputFormat = ImageFormat.Tiff;
                        break;
                    default:
                        outputFormat = ImageFormat.Jpeg;
                        break;
                }

                Convert(
                    options.SourceDir,
                    options.FileInputMask,
                    options.TargetDir, 
                    options.OverWrite, 
                    outputFormat,
                    options.MaxWidth,
                    options.MaxHeight,
                    options.WatermarkFile,
                    options.WatermarkOpacity);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                return 1;
            }
            return 0;
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        { }

        private static void Convert(
            string sourceDirectory,
            string fileInputMask,
            string targetDirectory,
            bool overwriteFlag,
            ImageFormat outputFormat,
            int maxWidth,
            int maxHeight,
            string watermarkfile,
            int opacityPercentage
            )
        {
            try
            {

                Bitmap waterMark = null;

                if (File.Exists(watermarkfile))
                {
                    try
                    {
                        waterMark = (opacityPercentage < 100) ?
                            ImageToolbox.ChangeOpacity(new Bitmap(watermarkfile), (float)opacityPercentage / 100) :
                            new Bitmap(watermarkfile);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Invalid watermark file: {watermarkfile}");
                        throw;
                    }
                }

                foreach (string imageFile in Directory.EnumerateFiles(sourceDirectory, fileInputMask, SearchOption.TopDirectoryOnly))
                {

                    Console.Write(imageFile);
                    string outputFilePath = Path.Combine(targetDirectory, Path.GetFileNameWithoutExtension(imageFile) + "." + outputFormat.ToString());

                    if (File.Exists(outputFilePath) && !overwriteFlag)
                    {
                        Console.WriteLine("\tSkipped: Target exists and overwrite flag is off");
                    }
                    else
                    {
                        // load the image
                        Bitmap bm = null;
                        bool isImage = false;
                        try
                        {
                            bm = new Bitmap(imageFile);
                            isImage = true;
                        }
                        catch (Exception)
                        {
                            if (bm != null)
                            {
                                bm.Dispose();
                            }
                        }
                        if (isImage)
                        {

                            float scale;
                            Bitmap bm2;

                            if (maxHeight > 0 && maxWidth > 0)
                            {
                                // find scale so image isn't larger than maxWidth X maxHeight
                                scale = Math.Min((float)maxWidth / bm.Width, (float)maxHeight / bm.Height);
                            }
                            else
                            {
                                scale = 1;
                            }
                            bm2 = ImageToolbox.ResizeTo(bm, bm.Width * scale, bm.Height * scale, true);
                            bm.Dispose();

                            if (waterMark != null)
                            {
                                // Redim watermark to superimpose on input image
                                scale = Math.Min((float)bm2.Width / waterMark.Width, (float)bm2.Height / waterMark.Height);
                                Bitmap wtBm2 = ImageToolbox.ResizeTo(waterMark, waterMark.Width * scale, waterMark.Height * scale, true);
                                ImageToolbox.Superimpose(bm2, wtBm2);
                                wtBm2.Dispose();
                            }

                            bm2.Save(outputFilePath, outputFormat);
                            bm2.Dispose();

                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine(" Not an image file.");
                        }
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
