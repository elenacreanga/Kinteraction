using System;
using Kinteract.Poses.LogicalPoses;
using Microsoft.Kinect;

namespace Kinteract.Poses
{
    public abstract class Posture
    {
        public abstract double Matches(Body body);

        public static implicit operator Func<Body, double>(Posture posture)
        {
            return posture.Matches;
        }

        public static explicit operator Posture(Func<Body, double> function)
        {
            return new FunctionalPosture(function);
        }

        public static Posture operator &(Posture p, Posture q)
        {
            return new AndPosture(p, q);
        }

        public static Posture operator |(Posture p, Posture q)
        {
            return new OrPosture(p, q);
        }

        public static Posture operator !(Posture p)
        {
            return new NotPosture(p);
        }
    }
}