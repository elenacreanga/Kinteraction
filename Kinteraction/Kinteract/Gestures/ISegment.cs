using Microsoft.Kinect;

namespace Kinteract.Gestures
{
    public interface ISegment
    {
        Outcome Check(Body body);
    }
}