using Kinteract.Poses;
using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class CrossedArmsSegment : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;
            var rightHandOverlapsWithLeftShoulder = Pose.RightHand.OverlapsWith(JointType.ShoulderLeft);
            var righElbowBelowHands = Pose.RightElbow.Below(JointType.HandRight);

            var rightHandPose = rightHandOverlapsWithLeftShoulder & righElbowBelowHands;

            if (rightHandPose.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Undetermined;
            }

            var leftHandOverlapsWithRightShoulder = Pose.LeftHand.OverlapsWith(JointType.ShoulderRight);
            var leftElbowBelowHands = Pose.LeftElbow.Below(JointType.HandLeft);

            var leftHandPose = leftHandOverlapsWithRightShoulder & leftElbowBelowHands;

            if (leftHandPose.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Undetermined;
            }

            var gesture = rightHandPose & leftHandPose;

            if (gesture.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }

            return outcome;
        }
    }
}
