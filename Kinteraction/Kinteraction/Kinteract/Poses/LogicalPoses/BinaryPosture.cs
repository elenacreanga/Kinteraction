using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Poses.LogicalPoses
{
    public abstract class BinaryPosture : Posture
    {
        protected BinaryPosture(Posture first, Posture second)
        {
            First = first;
            Second = second;
        }

        public Posture First { get; }

        public Posture Second { get; }

        public override double Matches(Body body)
        {
            return GetValue(First.Matches(body), Second.Matches(body));
        }

        public abstract double GetValue(double first, double second);
    }
}