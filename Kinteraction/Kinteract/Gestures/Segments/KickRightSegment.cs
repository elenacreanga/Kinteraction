using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class KickRightSegmentOne : ISegment
    {
        public Outcome Check(Body body)
        {
            return Outcome.Failed;
        }
    }

    public class KickRightSegmentTwo : ISegment
    {
        public Outcome Check(Body body)
        {
            return Outcome.Failed;
        }
    }
}
