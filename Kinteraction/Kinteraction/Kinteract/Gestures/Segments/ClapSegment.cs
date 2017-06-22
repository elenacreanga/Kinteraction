using Kinteraction.Kinteract.Poses;
using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Gestures.Segments
{
    public class FirstClapSegment : ISegment
    {
        public Outcome Check(Body body)
        {
            // Right and Left Hand in front of Shoulders
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z && body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
            {
                // Hands between shoulder and hip
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y &&
                    body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y && body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // Hands between shoulders
                    if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderRight].Position.X && body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderLeft].Position.X &&
                        body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ShoulderLeft].Position.X && body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderRight].Position.X)
                    {
                        // Hands very close
                        var handsVeryClose = Pose.RightHand.OverlapsWith(JointType.HandLeft);
                        if (handsVeryClose.Matches(body) > Constants.Tolerance)
                        {
                            return Outcome.Successful;
                        }

                        return Outcome.Undetermined;
                    }

                    return Outcome.Failed;
                }

                return Outcome.Failed;
            }

            return Outcome.Failed;
        }
    }
    public class SecondClapSegment : ISegment
    {
        public Outcome Check(Body body)
        {
            var outcome = Outcome.Successful;
            var handsSameHeight = Pose.RightHand.AtTheSameHeightOf(JointType.HandLeft);
            if (handsSameHeight.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }
            var rightHandRightOfSpineMid = Pose.RightHand.ToTheRightOf(JointType.SpineMid);
            var leftHandleftOfSpineMid = Pose.LeftHand.ToTheLeftOf(JointType.SpineMid);
            var handsAtDistance = rightHandRightOfSpineMid & leftHandleftOfSpineMid;
            if (handsAtDistance.Matches(body) > Constants.Tolerance)
            {
                outcome = Outcome.Successful;
            }
            return outcome;
        }
    }
}