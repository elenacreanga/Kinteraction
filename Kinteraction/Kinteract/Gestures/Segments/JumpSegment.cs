using Microsoft.Kinect;

namespace Kinteract.Gestures.Segments
{
    public class JumpSegmentOne : ISegment
    {
        public Outcome Check(Body body)
        {
            return Outcome.Failed;
        }
    }
}
