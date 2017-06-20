using System;

namespace Kinteraction.Kinteract.Poses.LogicalPoses
{
    public class AndPosture : BinaryPosture
    {
        public AndPosture(Posture first, Posture second) : base(first, second) { }

        public override double GetValue(double first, double second)
            => Math.Min(first, second);
    }
}
