using Kinteract.Poses;
using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class KickLeftSegmentOne : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;
            var leftShinInStraightAngleWithLeftHip = Pose.LeftShin.InStraightAngleWith(JointType.HipLeft);
            var leftThighInObtuseAngleWithSpineBase = Pose.LeftThigh.InObtuseAngleWith(JointType.SpineBase);
            var rightThighInRightAngleWithSpineBase = Pose.RightThigh.InRightAngleWith(JointType.SpineBase);

            var pose = leftShinInStraightAngleWithLeftHip & leftThighInObtuseAngleWithSpineBase &
                       rightThighInRightAngleWithSpineBase;

            if (pose.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }
            return outcome;
        }
    }

    public class KickLeftSegmentTwo : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;

            var leftShinInStraightAngleWithLeftHip = Pose.LeftShin.InStraightAngleWith(JointType.HipLeft);
            var leftThighInRightAngleWithSpineBase = Pose.LeftThigh.InRightAngleWith(JointType.SpineBase);
            var rightThighInRightAngleWithSpineBase = Pose.RightThigh.InRightAngleWith(JointType.SpineBase);
            if (rightThighInRightAngleWithSpineBase.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Undetermined;
            }
            var pose = leftShinInStraightAngleWithLeftHip & leftThighInRightAngleWithSpineBase &
                       rightThighInRightAngleWithSpineBase;
            if (pose.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }
            return outcome;
        }
    }
}
