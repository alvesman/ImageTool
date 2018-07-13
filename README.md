# ImageTool
<pre>
Convert images between formats / resize / apply watermark

Imagetool --help

  -s, --SourceDir           Required. Directory with input files to be processed.
  -m, --FileInputMask       Required. Mask input files to be processed. E.g.: *.tif
  -t, --TargetDir           Required. Directory for output.
  -o, --OutFormat           Required. Output format: [png | gif | bmp | tif | jpg]
  -w, --OverWrite           (Default: false) OverWriteFlag: [true | false]
  -k, --Watermark           (Default: ) Watermark PNG file
  -p, --WatermarkOpacity    (Default: 100) Watermark opacity (0-100)
  -x, --MaxWidth            (Default: 0) Target max Width
  -y, --MaxHeight           (Default: 0) Target max Height
  --help                    Display this help screen.
  --version                 Display version information.
</pre>