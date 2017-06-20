using Kinteraction.Kinteract.Poses;
using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Gestures.Segments
{
    public class FirstWaveSegment : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;
            var handAboveElbow = Pose.RightHand.Above(JointType.ElbowRight);

            if (handAboveElbow.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Undetermined;
            }
            var handRightOfElbow = Pose.RightHand.ToTheRightOf(JointType.ElbowRight);

            var gesture = handAboveElbow & handRightOfElbow;
            if (gesture.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }
            return outcome;
        }
    }
    public class SecondWaveSegment : ISegment
    {

        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;
            var handAboveElbow = Pose.RightHand.Above(JointType.ElbowRight);

            if (handAboveElbow.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Undetermined;
            }
            var handLeftOfElbow = Pose.RightHand.ToTheLeftOf(JointType.ElbowRight);

            var gesture = handAboveElbow & handLeftOfElbow;
            if (gesture.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }

            return outcome;
        }
    }
}
