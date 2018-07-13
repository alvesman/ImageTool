using System;
using System.IO;
using System.Drawing; // reference System.Drawing
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ImageTool
{
   public static class ImageToolbox
    {

        public static Bitmap ResizeTo(Bitmap image, float widthTarget, float heightTarget, bool HighQualityInterpolation)
        {
            try
            {

                float scale = Math.Min((float)widthTarget / image.Width, (float)heightTarget / image.Height);

                var bmp = new Bitmap((int)widthTarget, (int)heightTarget);
                var graph = Graphics.FromImage(bmp);
                var brush = new SolidBrush(Color.Transparent);

                if (HighQualityInterpolation)
                {
                    graph.InterpolationMode = InterpolationMode.High;
                    graph.CompositingQuality = CompositingQuality.HighQuality;
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                }

                int scaleWidth = (int)(image.Width * scale);
                int scaleHeight = (int)(image.Height * scale);

                graph.FillRectangle(brush, new RectangleF(0, 0, widthTarget, heightTarget));
                graph.DrawImage(image, ((int)widthTarget - scaleWidth) / 2, ((int)heightTarget - scaleHeight) / 2, scaleWidth, scaleHeight);

                return bmp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Bitmap Superimpose(Bitmap original, Bitmap waterMark)
        {
            const int pixelMargin = 3;
            Graphics g = Graphics.FromImage(original);
            g.CompositingMode = CompositingMode.SourceOver;
            int x = original.Width / 2 - waterMark.Width / 2 - pixelMargin;
            int y = original.Height / 2 - waterMark.Height / 2 - pixelMargin;
            g.DrawImage(waterMark, new Point(x, y));
            g.Dispose();
            return original;
        }

        public static Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            // https://docs.microsoft.com/en-us/dotnet/framework/winforms/advanced/how-to-use-a-color-matrix-to-set-alpha-values-in-images
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix
            {
                Matrix33 = opacityvalue
            };
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();
            return bmp;
        }

    }
}
