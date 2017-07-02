using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class SquatSegmentOne : ISegment
    {
        public Outcome Check(Body body)
        {
            return Outcome.Failed;
        }
    }

    public class SquatSegmentTwo : ISegment
    {
        public Outcome Check(Body body)
        {
            return Outcome.Failed;
        }
    }
}