using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Poses.LogicalPoses
{
    public class NotPosture : Posture
    {
        public NotPosture(Posture operand)
        {
            Operand = operand;
        }

        public Posture Operand { get; }

        public override double Matches(Body body)
        {
            return 1 - Operand.Matches(body);
        }
    }
}