using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace Kinteraction.Helpers
{
    public static class ImageExtensions
    {
        private static readonly ColorBitmapGenerator ColorBitmapGenerator = new ColorBitmapGenerator();
        private static readonly DepthBitmapGenerator DepthBitmapGenerator = new DepthBitmapGenerator();

        public static WriteableBitmap ToBitmap(this ColorFrame frame)
        {
            ColorBitmapGenerator.UpdateBitmap(frame);

            return ColorBitmapGenerator.Bitmap;
        }

        public static WriteableBitmap ToBitmap(this DepthFrame frame)
        {
            DepthBitmapGenerator.UpdateBitmap(frame);

            return DepthBitmapGenerator.Bitmap;
        }
    }
}