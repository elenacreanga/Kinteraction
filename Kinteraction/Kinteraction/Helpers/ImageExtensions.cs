using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace Kinteraction.Helpers
{
    public static class ImageExtensions
    {
        private static readonly ColorBitmapGenerator ColorBitmapGenerator = new ColorBitmapGenerator();

        public static WriteableBitmap ToBitmap(this ColorFrame frame)
        {
            ColorBitmapGenerator.UpdateBitmap(frame);

            return ColorBitmapGenerator.Bitmap;
        }

    }
}
