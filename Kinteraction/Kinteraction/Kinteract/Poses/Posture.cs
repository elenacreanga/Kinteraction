using System;
using Kinteraction.Kinteract.Poses.LogicalPoses;
using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Poses
{
    public abstract class Posture
    {
        public abstract double Matches(Body body);

        public static implicit operator Func<Body, double>(Posture posture)
        {
            return (body) => posture.Matches(body);
        }

        public static explicit operator Posture(Func<Body, double> function)
            => new FunctionalPosture(function);

        public static Posture operator &(Posture p, Posture q) => new AndPosture(p, q);

        public static Posture operator |(Posture p, Posture q) => new OrPosture(p, q);

    }
}
