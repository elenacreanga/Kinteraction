using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Gestures
{
    public interface ISegment
    {
        Outcome Check(Body body);
    }
}
