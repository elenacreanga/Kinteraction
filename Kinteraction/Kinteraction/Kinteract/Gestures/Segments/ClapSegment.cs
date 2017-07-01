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
            var handsVeryClose = Pose.RightHand.OverlapsWith(JointType.HandLeft);
            if (handsVeryClose.Matches(body) >= Constants.Tolerance)
            {
                return Outcome.Undetermined;
                
            }
            var handsAtTheSameHeight = Pose.RightHand.AtTheSameHeightOf(JointType.HandLeft);
            var handsAtTheSameDepth = Pose.RightHand.AtTheSameDepthOf(JointType.HandLeft);
            var handsAtTheSameHeightAndDepth = handsAtTheSameHeight & handsAtTheSameDepth;

            if (handsAtTheSameHeightAndDepth.Matches(body) > Constants.Tolerance)
            {
                return Outcome.Successful;
            }
            return Outcome.Failed;
        }
    }
}