using System;

namespace Kinteraction.Kinteract.Poses.LogicalPoses
{
    public class OrPosture : BinaryPosture
    {
        public OrPosture(Posture first, Posture second) : base(first, second) { }

        public override double GetValue(double first, double second)
            => Math.Max(first, second);
    }
}
