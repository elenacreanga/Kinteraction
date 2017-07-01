using System;
using Microsoft.Kinect;

namespace Kinteract.Poses
{
    public class FunctionalPosture : Posture
    {
        public FunctionalPosture(Func<Body, double> function)
        {
            Function = function;
        }

        public Func<Body, double> Function { get; }

        public override double Matches(Body body)
        {
            return Function(body);
        }
    }
}