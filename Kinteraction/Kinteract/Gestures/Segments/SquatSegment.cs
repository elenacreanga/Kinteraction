using Kinteract.Poses;
using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class SquatSegmentOne : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;
            var leftThighInStraightAngleWithHip = Pose.LeftThigh.InStraightAngleWith(JointType.SpineBase);
            var rightThighInStraightAngleWithHip = Pose.RightThigh.InStraightAngleWith(JointType.SpineBase);
            var poseStraightAngle = rightThighInStraightAngleWithHip & leftThighInStraightAngleWithHip;

            //var leftThighInObtuseAngleWithHip = Pose.LeftThigh.InObtuseAngleWith(JointType.SpineBase);
            //var rightThighInObtuseAngleWithHip = Pose.RightThigh.InObtuseAngleWith(JointType.SpineBase);
            //var poseObtuseAngle = rightThighInObtuseAngleWithHip & leftThighInObtuseAngleWithHip;

            var pose = poseStraightAngle ;//| poseObtuseAngle;

            if (pose.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }
            return outcome;
        }
    }

    public class SquatSegmentTwo : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Undetermined;
            var rightKneeAtTheSameDepthWithSpineBase = Pose.HipCenter.AtTheSameDepthOf(JointType.KneeRight);

            var leftKneeAtTheSameDepthWithSpineBase = Pose.HipCenter.AtTheSameDepthOf(JointType.KneeLeft);

            var posture = leftKneeAtTheSameDepthWithSpineBase & rightKneeAtTheSameDepthWithSpineBase;

            if (posture.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }

            return outcome;
        }
    }
}