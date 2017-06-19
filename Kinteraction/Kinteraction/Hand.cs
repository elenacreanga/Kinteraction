using System.Windows.Media.Media3D;

namespace Kinteraction
{
    public class Hand
    {
        public bool IsOpen { get; set; }
        public float[] Origin { get; set; }

        public Hand()
        {
            IsOpen = true;
            Origin = new float[3];
        }

    }
}
