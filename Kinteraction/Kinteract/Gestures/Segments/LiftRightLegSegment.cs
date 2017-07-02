using Kinteract.Poses;
using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class LiftRightLegSegmentOne : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;

            var rightKneeInFrontOfSpineBase = Pose.RightKnee.Before(JointType.SpineBase);

            var leftKneeAtTheSameDepthOfSpineBase = Pose.HipCenter.AtTheSameDepthOf(JointType.KneeLeft);

            var rightShinInRightAngleWithRightHip = Pose.RightShin.InRightAngleWith(JointType.HipRight);
            var rightShinInObtuseAngleWithRightHip = Pose.RightShin.InObtuseAngleWith(JointType.HipRight);
            var anglePosture = (rightShinInObtuseAngleWithRightHip | rightShinInRightAngleWithRightHip);
            var depthPosture = leftKneeAtTheSameDepthOfSpineBase & rightKneeInFrontOfSpineBase;

            if (depthPosture.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }

            return outcome;
        }
    }
    public class LiftRightLegSegmentTwo : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;
            var rightKneeAtTheSameDepthWithSpineBase = Pose.HipCenter.AtTheSameDepthOf(JointType.KneeRight);

            var leftKneeAtTheSameDepthWithSpineBase = Pose.HipCenter.AtTheSameDepthOf(JointType.KneeLeft);

            if (leftKneeAtTheSameDepthWithSpineBase.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Undetermined;
            }

            var posture = leftKneeAtTheSameDepthWithSpineBase & rightKneeAtTheSameDepthWithSpineBase;

            if (posture.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }

            return outcome;
        }
    }
}
