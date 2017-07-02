using Kinteract.Poses;
using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class SurrenderSegment : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;
            var rightHandAboveRightElbow = Pose.RightHand.Above(JointType.ElbowRight);

            var rightElbowToTheRightOfRightShoulder = Pose.RightElbow.ToTheRightOf(JointType.ShoulderRight);
            var rightForeArmInRightAngleWithRightShoulder = Pose.RightForearm.InRightAngleWith(JointType.ShoulderRight);

            var rightElbowInStraightAngleWithRightClavicule = Pose.RightClavicle.InStraightAngleWith(JointType.ElbowRight);

            var rightHandPose = rightHandAboveRightElbow & rightElbowToTheRightOfRightShoulder
                                & rightForeArmInRightAngleWithRightShoulder
                                & rightElbowInStraightAngleWithRightClavicule;

            if (rightHandPose.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Undetermined;
            }

            var leftHandAboveLeftElbow = Pose.LeftHand.Above(JointType.ElbowLeft);
            var leftElbowToTheLeftOfLeftShoulder = Pose.LeftElbow.ToTheLeftOf(JointType.ShoulderLeft);

            var leftElbowInStraightAngleWithLeftClavicule = Pose.LeftClavicle.InStraightAngleWith(JointType.ElbowLeft);

            var leftForeArmInRightAngleWithLeftShoulder = Pose.LeftForearm.InRightAngleWith(JointType.ShoulderLeft);

            var leftHandPose = leftHandAboveLeftElbow & leftElbowToTheLeftOfLeftShoulder
            & leftForeArmInRightAngleWithLeftShoulder
                & leftElbowInStraightAngleWithLeftClavicule;

            if (leftHandPose.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Undetermined;
            }

            var pose = rightHandPose & leftHandPose;

            if (pose.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }

            return outcome;
        }
    }
}
