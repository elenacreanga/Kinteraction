using System;
using Kinteract.Poses.Distance;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;

namespace Kinteract.Poses.Helpers
{
    internal static class DistanceExtensions
    {
        public static bool WithTolerance(this double value, double tolerance)
        {
            return Math.Abs(value) <= tolerance;
        }

        public static double DistanceTo(this CameraSpacePoint first, CameraSpacePoint second)
        {
            double delthaX = first.X - second.X;
            double delthaY = first.Y - second.Y;
            double delthaZ = first.Z - second.Z;
            return Math.Sqrt(delthaX * delthaX + delthaY * delthaY + delthaZ * delthaZ);
        }

        public static CameraSpacePoint TranslateTo(this CameraSpacePoint point, CameraSpacePoint origin)
        {
            var result = new CameraSpacePoint();
            result.X = origin.X - point.X;
            result.Y = origin.Y - point.Y;
            result.Z = origin.Z - point.Z;

            return result;
        }

        public static double Length(this CameraSpacePoint point)
        {
            return Math.Sqrt(point.X * point.X + point.Y * point.Y + point.Z * point.Z);
        }


        public static double DotProduct(this CameraSpacePoint one, CameraSpacePoint other)
        {
            return one.X * other.X + one.Y * other.Y + one.Z * other.Z;
        }

        public static double DistanceTo(this Joint first, Joint second)
        {
            return first.Position.DistanceTo(second.Position);
        }

        public static double Holds(this Body body, Posture posture)
        {
            return posture.Matches(body);
        }


        public static bool Holds(this Body body, Posture posture, double tolerance)
        {
            return body.Holds(posture).WithTolerance(tolerance);
        }

        public static double DistanceBetween(this Body body, JointType first, JointType second)
        {
            return body.Joints[first].Position.DistanceTo(body.Joints[second].Position);
        }

        public static double PathLengthBetween(this Body body, JointType first, JointType second)
        {
            var pathD = new Dijkstra().CalculateDistance(second, first);
            double length = 0;
            for (var i = 1; i < pathD.Count; i++)
            {
                var jointiMinusOne = (JointType) Enum.Parse(typeof(JointType), pathD[i - 1]);
                var jointI = (JointType) Enum.Parse(typeof(JointType), pathD[i]);
                length += body.Joints[jointiMinusOne].DistanceTo(body.Joints[jointI]);
            }
            var x = body.Joints[first].DistanceTo(body.Joints[second]);
            var y = length + body.Joints[second]
                        .DistanceTo(body.Joints[(JointType) Enum.Parse(typeof(JointType), pathD[0])]);
            return length;
        }
        public static double PathLength(this Body body, JointType first, JointType second)
        {
            var pathD = new Dijkstra().CalculateDistance(second, first);
            double length = 0;
            for (var i = 1; i < pathD.Count; i++)
            {
                var jointiMinusOne = (JointType) Enum.Parse(typeof(JointType), pathD[i - 1]);
                var jointI = (JointType) Enum.Parse(typeof(JointType), pathD[i]);
                length += body.Joints[jointiMinusOne].DistanceTo(body.Joints[jointI]);
            }
            var x = body.Joints[first].DistanceTo(body.Joints[second]);
            var y = length + body.Joints[second]
                        .DistanceTo(body.Joints[(JointType) Enum.Parse(typeof(JointType), pathD[0])]);
            var distanceBetween = (length + x) / 2;
            return distanceBetween;
        }

        public static double XDiff(this Body body, JointType first, JointType second)
        {
            if (body.Joints[first].Position.X < 0)
            {
                Console.WriteLine(body.Joints[first].Position.X);
            }
            return body.Joints[first].Position.X - body.Joints[second].Position.X;
        }

        public static double YDiff(this Body body, JointType first, JointType second)
        {
            return body.Joints[first].Position.Y - body.Joints[second].Position.Y;
        }

        public static double ZDiff(this Body body, JointType first, JointType second)
        {
            return body.Joints[first].Position.Z - body.Joints[second].Position.Z;
        }

        public static double GetAngleBetween(this Body body, JointType first, JointType second, JointType third)
        {
            var start = body.Joints[first].Position.To3DVector();
            var middle = body.Joints[second].Position.To3DVector();
            var end = body.Joints[third].Position.To3DVector();
            var vectorOne = middle - start;
            var vectorTwo = middle - end;

            vectorOne.Normalize();
            vectorTwo.Normalize();

            return Vector3D.AngleBetween(vectorOne, vectorTwo);
        }
    }
}