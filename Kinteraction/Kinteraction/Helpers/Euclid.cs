using System;

namespace Kinteraction.Helpers
{
    public static class Euclid
    {
        public static float Calculate(float[] a, float[] b)
        {
            return (float)Math.Sqrt((a[0] - b[0]) * (a[0] - b[0]) + (a[1] - b[1]) * (a[1] - b[1]) +
                                    (a[2] - b[2]) * (a[2] - b[2]));
        }
    }
}