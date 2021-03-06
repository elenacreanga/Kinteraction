﻿using System;

namespace Kinteract.Poses.Helpers
{
    internal static class RatioCalculator
    {
        public static double DifferenceDistanceRatio(double difference, double distance)
        {
            return difference < 0 ? 0 : Math.Min(1, difference / distance);
        }

        public static double AbsoluteDifferenceDistanceRatio(double difference, double distance)
        {
            return 1- Math.Abs(difference) / distance;
        }

        public static double DifferenceDistance(double x, double y, double z)
        {
            return 1 - (Math.Abs(x) + Math.Abs(y) + Math.Abs(z));
        }

        public static double AngleDifference(double desiredAngle, double actual)
        {
            return 1 - Math.Abs(desiredAngle - actual)/100;
        }
    }
}