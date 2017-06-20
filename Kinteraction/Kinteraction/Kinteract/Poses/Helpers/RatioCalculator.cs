using System;

namespace Kinteraction.Kinteract.Poses.Helpers
{
    internal static class RatioCalculator
    {
        public static double DifferenceDistanceRatio(double difference, double distance)
        {
            return difference < 0 ? 0 : Math.Min(1, difference / distance);
        }

        public static double AbsoluteDifferenceDistanceRatio(double difference, double distance)
            => Math.Abs(difference) / distance;
    }
}
