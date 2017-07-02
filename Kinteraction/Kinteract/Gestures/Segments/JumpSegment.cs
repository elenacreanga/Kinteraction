using System;
using Kinteract.Poses;
using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class JumpSegmentOne : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;
            var leftShinInObtuseAngleWithLeftHip = Pose.LeftShin.InObtuseAngleWith(JointType.HipLeft);
            var rightShinInObtuseAngleWithRightHip = Pose.RightShin.InObtuseAngleWith(JointType.HipRight);

            var leftThighInObtuseAngleWithHip = Pose.LeftThigh.InObtuseAngleWith(JointType.SpineBase);
            var rightThighInObtuseAngleWithHip = Pose.RightThigh.InObtuseAngleWith(JointType.SpineBase);
            var poseObtuseAngle = rightThighInObtuseAngleWithHip & leftThighInObtuseAngleWithHip;


            var pose = (leftShinInObtuseAngleWithLeftHip & rightShinInObtuseAngleWithRightHip) | poseObtuseAngle;
            if (pose.Matches(body) > 0.7)
            {
                return Outcome.Successful;
            }
            return outcome;
        }
    }
    public class JumpSegmentTwo : ISegment
    {
        public float Height;

        public Outcome Check(Body body)
        {
            var outcome = Outcome.Undetermined;
            var leftShinInStraightAngleWithLeftHip = Pose.LeftShin.InStraightAngleWith(JointType.HipLeft);
            var rightShinInStraightAngleWithRightHip = Pose.RightShin.InStraightAngleWith(JointType.HipRight);
            var pose = rightShinInStraightAngleWithRightHip & leftShinInStraightAngleWithLeftHip;
            if (pose.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
                Height = body.Joints[JointType.Head].Position.X;
            }
            return outcome;
        }
    }

    public class JumpSegmentThree : ISegment
    {
        public DateTime Timestamp;
        private readonly JumpSegmentTwo _jumpSegmentTwo;

        public JumpSegmentThree(JumpSegmentTwo jumpSegmentTwo)
        {
            _jumpSegmentTwo = jumpSegmentTwo;
        }

        public Outcome Check(Body body)
        {
            var outcome = Outcome.Undetermined;
            var leftShinInStraightAngleWithLeftHip = Pose.LeftShin.InStraightAngleWith(JointType.HipLeft);
            var rightShinInStraightAngleWithRightHip = Pose.RightShin.InStraightAngleWith(JointType.HipRight);
            var pose = rightShinInStraightAngleWithRightHip & leftShinInStraightAngleWithLeftHip;
            var distanceBetweenJumpAndNormalState = _jumpSegmentTwo.Height - body.Joints[JointType.Head].Position.X;
            if (pose.Matches(body) > Constants.Tolerance && distanceBetweenJumpAndNormalState > 0)
            {
                return Outcome.Successful;
            }

            return outcome;
        }
    }
}
