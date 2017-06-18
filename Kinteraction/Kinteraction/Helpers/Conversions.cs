using System.Collections.Generic;

namespace Kinteraction.Helpers
{
    public static class Conversions
    {
        public static double[] ToDoubles(this float[] floats)
        {
            var doubles = new List<double>();
            foreach (var f in floats)
            {
                var parsedDouble = System.Convert.ToDouble(f);
                doubles.Add(parsedDouble);
            }
            return doubles.ToArray();
        }
    }
}
