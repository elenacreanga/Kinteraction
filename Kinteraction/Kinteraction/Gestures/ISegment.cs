using Microsoft.Kinect;

namespace Kinteraction.Gestures
{
    public interface ISegment
    {
        Outcome Check(Body body);
    }
}
