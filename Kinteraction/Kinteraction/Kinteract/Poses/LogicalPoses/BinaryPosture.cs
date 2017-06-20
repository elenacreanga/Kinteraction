using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Poses.LogicalPoses
{
    public abstract class BinaryPosture : Posture
    {
        public BinaryPosture(Posture first, Posture second)
        {
            First = first;
            Second = second;
        }

        public Posture First { get; private set; }

        public Posture Second { get; private set; }

        public override double Matches(Body body)
            => GetValue(First.Matches(body), Second.Matches(body));

        public abstract double GetValue(double first, double second);
    }
}
