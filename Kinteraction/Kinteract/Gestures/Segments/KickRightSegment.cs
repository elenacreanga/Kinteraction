using Kinteract.Poses;
using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class KickRightSegmentOne : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;
            var rightShinInStraightAngleWithRightHip = Pose.RightShin.InStraightAngleWith(JointType.HipRight);
            var rightThighInObtuseAngleWithSpineBase = Pose.RightThigh.InObtuseAngleWith(JointType.SpineBase);
            var leftThighInRightAngleWithSpineBase = Pose.LeftThigh.InRightAngleWith(JointType.SpineBase);

            var pose = rightShinInStraightAngleWithRightHip & rightThighInObtuseAngleWithSpineBase &
                       leftThighInRightAngleWithSpineBase;

            if (pose.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }
            return outcome;
        }
    }

    public class KickRightSegmentTwo : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;

            var rightShinInStraightAngleWithRightHip = Pose.RightShin.InStraightAngleWith(JointType.HipRight);
            var rightThighInRightAngleWithSpineBase = Pose.RightThigh.InRightAngleWith(JointType.SpineBase);
            var leftThighInRightAngleWithSpineBase = Pose.LeftThigh.InRightAngleWith(JointType.SpineBase);
            if (leftThighInRightAngleWithSpineBase.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Undetermined;
            }
            var pose = rightShinInStraightAngleWithRightHip & rightThighInRightAngleWithSpineBase &
                       leftThighInRightAngleWithSpineBase;
            if (pose.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }
            return outcome;
        }
    }
}
