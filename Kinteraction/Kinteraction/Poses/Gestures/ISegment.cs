using Microsoft.Kinect;

namespace Kinteraction.Poses.Gestures
{
    public interface ISegment
    {
        Outcome Check(Body body);
    }
}
