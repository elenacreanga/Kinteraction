using System;
using Kinteraction.Kinteract.Poses.Distance;
using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Poses.Helpers
{
    internal static class KinectExtension
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

        public static double AngleTo(this CameraSpacePoint point, CameraSpacePoint other)
        {
            return Math.Acos(point.DotProduct(other) / (point.Length() * other.Length()));
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
            var pathD = new Dijkstra().CalculateDistance(first, second);
            double length = 0;
            for (var i = 1; i < pathD.Count; i++)
            {
                var jointiMinusOne = (JointType) Enum.Parse(typeof(JointType), pathD[i - 1]);
                var jointI = (JointType) Enum.Parse(typeof(JointType), pathD[i]);
                length += body.Joints[jointiMinusOne].DistanceTo(body.Joints[jointI]);
            }

            //var path = JointPath.Between(first, second);
            //double length = 0;
            //for (var i = 1; i < path.Count; i++)
            //{
            //    var jointiminusOne = path[i - 1];
            //    var jointI = path[i];
            //    length += body.Joints[jointiminusOne].DistanceTo(body.Joints[jointI]);
            //}
            return length;
        }

        public static double XDiff(this Body body, JointType first, JointType second)
        {
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
    }
}