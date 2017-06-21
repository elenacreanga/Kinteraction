using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace Kinteraction.Helpers
{
    public class ColorBitmapGenerator
    {
        public byte[] RgbPixels { get; protected set; }

        public int BitmapWidth { get; protected set; }

        public int BitmanHeight { get; protected set; }

        public WriteableBitmap Bitmap { get; protected set; }

        public void UpdateBitmap(ColorFrame frame)
        {
            if (Bitmap == null)
            {
                BitmapWidth = frame.FrameDescription.Width;
                BitmanHeight = frame.FrameDescription.Height;
                RgbPixels = new byte[BitmapWidth * BitmanHeight * Constants.BYTES_PER_PIXEL];
                Bitmap = new WriteableBitmap(BitmapWidth, BitmanHeight, Constants.DPI, Constants.DPI, Constants.FORMAT,
                    null);
            }

            frame.CopyConvertedFrameDataToArray(RgbPixels, ColorImageFormat.Bgra);

            Bitmap.Lock();

            Marshal.Copy(RgbPixels, 0, Bitmap.BackBuffer, RgbPixels.Length);
            Bitmap.AddDirtyRect(new Int32Rect(0, 0, BitmapWidth, BitmanHeight));

            Bitmap.Unlock();
        }
    }
}