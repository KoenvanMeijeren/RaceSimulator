using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFRaceSimulator
{
    /// <summary>
    /// Provides a class for creating, loading and editing images. E.g. rotating or mirroring images.
    /// </summary>
    public static class WPFImageBuilder
    {

        private static Dictionary<string, Bitmap> CachedImages;

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
                    bitmapData.Stride);
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
        
        public static Bitmap LoadImage(string url)
        {
            if (!WPFImageBuilder.CachedImages.ContainsKey(url))
            {
                WPFImageBuilder.CachedImages.Add(url, new Bitmap(Image.FromFile(url)));
            }

            return (Bitmap) WPFImageBuilder.CachedImages[url].Clone();
        }

    }
}