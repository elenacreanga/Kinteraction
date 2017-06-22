using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace Kinteraction.Helpers
{
    public class DepthBitmapGenerator
    {
        public byte[] HighlightedRgbPixels { get; protected set; }
        public byte[] RgbPixels { get; protected set; }
        public WriteableBitmap HighlightedBitmap { get; protected set; }
        public ushort[] DepthData { get; protected set; }
        public byte[] BodyData { get; protected set; }
        public int Width { get; protected set; }

        public int Height { get; protected set; }
        public WriteableBitmap Bitmap { get; protected set; }

        public void UpdateBitmap(DepthFrame frame)
        {
            var minDepth = frame.DepthMinReliableDistance;
            var maxDepth = frame.DepthMaxReliableDistance;

            if (Bitmap == null)
            {
                Width = frame.FrameDescription.Width;
                Height = frame.FrameDescription.Height;
                DepthData = new ushort[Width * Height];
                RgbPixels = new byte[Width * Height * Constants.BYTES_PER_PIXEL];
                Bitmap = new WriteableBitmap(Width, Height, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
            }

            frame.CopyFrameDataToArray(DepthData);

            var colorIndex = 0;

            foreach (var depth in DepthData)
            {
                var intensity = (byte) (depth >= minDepth && depth <= maxDepth ? depth : 0);

                RgbPixels[colorIndex++] = intensity; // Blue
                RgbPixels[colorIndex++] = intensity; // Green
                RgbPixels[colorIndex++] = intensity; // Red

                ++colorIndex;
            }

            Bitmap.Lock();

            Marshal.Copy(RgbPixels, 0, Bitmap.BackBuffer, RgbPixels.Length);
            Bitmap.AddDirtyRect(new Int32Rect(0, 0, Width, Height));

            Bitmap.Unlock();
        }
    }
}