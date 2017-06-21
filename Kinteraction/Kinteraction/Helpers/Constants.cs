using System.Windows.Media;

namespace Kinteraction.Helpers
{
    public static class Constants
    {
        public static readonly string HandPosition = "Hand Position";
        public static readonly string NotDetected = "Not detected";
        public static readonly string HandStatus = "Hand Status";
        public static readonly string Detected = "Detected";
        public static readonly double DPI = 96.0;
        public static readonly PixelFormat FORMAT = PixelFormats.Bgr32;
        public static readonly int BYTES_PER_PIXEL = (FORMAT.BitsPerPixel + 7) / 8;
    }
}