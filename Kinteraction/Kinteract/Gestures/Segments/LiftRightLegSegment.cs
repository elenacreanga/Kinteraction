using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class LiftRightLegSegmentOne : ISegment
    {
        public Outcome Check(Body body)
        {
            return Outcome.Failed;
        }
    }
    public class LiftRightLegSegmentTwo : ISegment
    {
        public Outcome Check(Body body)
        {
            return Outcome.Failed;
        }
    }
}
