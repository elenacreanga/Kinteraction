using Kinteract.Poses;
using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class LiftLeftLegSegmentOne : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;

            var leftKneeInFrontOfSpineBase = Pose.LeftKnee.Before(JointType.SpineBase);

            var rightKneeAtTheSameDepthOfSpineBase = Pose.HipCenter.AtTheSameDepthOf(JointType.KneeRight);
            var posture = rightKneeAtTheSameDepthOfSpineBase & leftKneeInFrontOfSpineBase;

            if (posture.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }

            return outcome;
        }
    }
    public class LiftLeftLegSegmentTwo : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;
            var rightKneeAtTheSameDepthWithSpineBase = Pose.HipCenter.AtTheSameDepthOf(JointType.KneeRight);

            var leftKneeAtTheSameDepthWithSpineBase = Pose.HipCenter.AtTheSameDepthOf(JointType.KneeLeft);

            if (rightKneeAtTheSameDepthWithSpineBase.Matches(body) > Constants.Tolerance)
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
