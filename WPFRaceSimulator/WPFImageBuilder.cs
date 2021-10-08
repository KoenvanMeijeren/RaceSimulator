using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace WPFRaceSimulator
{
    /// <summary>
    /// Provides a class for creating, loading and editing images. E.g. rotating or mirroring images.
    /// </summary>
    public static class WPFImageBuilder
    {
        private const int ImageWidthUndefined = -1;

        private const string MainBitmapKey = "empty";
        
        private static readonly Dictionary<string, Bitmap> CachedImages = new Dictionary<string, Bitmap>();

        public static Bitmap CreateBitmap(int width, int height)
        {
            if (WPFImageBuilder.CachedImages.TryGetValue(WPFImageBuilder.MainBitmapKey, out var bitmap))
            {
                return (Bitmap) bitmap.Clone();
            }
            
            bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Red);

            WPFImageBuilder.CachedImages.Add(WPFImageBuilder.MainBitmapKey, bitmap);

            return bitmap;
        }
        
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }

            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                int size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride
                );
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
        
        public static void ClearCache()
        {
            WPFImageBuilder.CachedImages.Clear();
        }

        public static Bitmap LoadImage(string url, int width = WPFImageBuilder.ImageWidthUndefined, int height = WPFImageBuilder.ImageWidthUndefined)
        {
            if (WPFImageBuilder.CachedImages.TryGetValue(url, out var bitmap))
            {
                return (Bitmap)bitmap.Clone();
            }

            if (width != WPFImageBuilder.ImageWidthUndefined && height != WPFImageBuilder.ImageWidthUndefined)
            {
                bitmap = new Bitmap(Image.FromFile(url), width, height);
            }
            else
            {
                bitmap = new Bitmap(Image.FromFile(url));
            }
            
            WPFImageBuilder.CachedImages.Add(url, bitmap);

            return (Bitmap)bitmap.Clone();
        }

    }
}