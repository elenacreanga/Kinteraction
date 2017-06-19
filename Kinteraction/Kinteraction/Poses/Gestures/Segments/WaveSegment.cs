using Microsoft.Kinect;

namespace Kinteraction.Poses.Gestures.Segments
{
    public class FirstWaveSegment : ISegment
    {
        public Outcome Check(Body body)
        {
            // Hand above elbow
            if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
            {
                // Hand right of elbow
                if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ElbowRight].Position.X)
                {
                    return Outcome.Successful;
                }

                // Hand has not dropped but is not quite where we expect it to be, pausing till next frame
                return Outcome.Undetermined;
            }

            // Hand dropped - no gesture fails
            return Outcome.Failed;
        }
    }
    public class SecondWaveSegment : ISegment
    {
        public Outcome Check(Body body)
        {
            // Hand above elbow
            if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
            {
                // Hand right of elbow
                if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ElbowRight].Position.X)
                {
                    return Outcome.Successful;
                }

                // Hand has not dropped but is not quite where we expect it to be, pausing till next frame
                return Outcome.Undetermined;
            }

            // Hand dropped - no gesture fails
            return Outcome.Failed;
        }
    }
}
