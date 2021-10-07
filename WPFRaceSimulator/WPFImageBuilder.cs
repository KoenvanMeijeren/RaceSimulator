using System.Collections.Generic;
using System.Drawing;

namespace WPFRaceSimulator
{
    /// <summary>
    /// Provides a class for creating, loading and editing images. E.g. rotating or mirroring images.
    /// </summary>
    public static class WPFImageBuilder
    {

        private static Dictionary<string, Bitmap> CachedImages;

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

            return WPFImageBuilder.CachedImages[url];
        }
        
    }
}