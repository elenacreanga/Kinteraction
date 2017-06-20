using System;
using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Poses
{
    public class FunctionalPosture : Posture
    {

        public FunctionalPosture(Func<Body, double> function)
        {
            Function = function;
        }

        public Func<Body, double> Function { get; private set; }

        public override double Matches(Body body)
            => Function(body);
    }
}
