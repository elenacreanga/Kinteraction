using Kinteraction.Kinteract.Poses;
using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Gestures.Segments
{
    public class CrossedArmsSegment : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Failed;

            var rightHandatTheSameHeightOf = Pose.RightHand.AtTheSameHeightOf(JointType.ShoulderLeft);
            var atshom = rightHandatTheSameHeightOf.Matches(body);
            var theLeftOf = Pose.RightHand.ToTheRightOf(JointType.SpineShoulder);
            var tlom = theLeftOf.Matches(body);
            var atTheSameDepthOf = Pose.RightHand.AtTheSameDepthOf(JointType.ElbowRight);
            var atsdom = atTheSameDepthOf.Matches(body);

            var rightHandPose = //rightHandBeforeShoulderLeft &
                rightHandatTheSameHeightOf &
                theLeftOf &
                atTheSameDepthOf;


            if (rightHandPose.Matches(body) > Constants.Tolerance)
                outcome = Outcome.Successful;
            //var leftHandPose = Pose.LeftHand.Before(JointType.ShoulderRight)
            //                   & Pose.LeftHand.AtTheSameHeightOf(JointType.ShoulderRight)
            //                   & Pose.LeftHand.ToTheLeftOf(JointType.SpineShoulder)
            //                   & Pose.LeftHand.AtTheSameDepthOf(JointType.ElbowRight);

            //if (leftHandPose.Matches(body) > Constants.Tolerance)
            //{
            //    outcome = Outcome.Undetermined;
            //}

            //var gesture = leftHandPose & rightHandPose;
            //if (gesture.Matches(body) > Constants.Tolerance)
            //{
            //    outcome = Outcome.Successful;
            //}

            return outcome;
        }
    }
}