using System;
using System.Linq;

namespace Kinteraction.Helpers
{
    public static class Conversions
    {
        public static double[] ToDoubles(this float[] floats)
        {
            return floats.Select(Convert.ToDouble).ToArray();
        }
    }
}