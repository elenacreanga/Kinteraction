using System.Windows.Media.Media3D;
using Microsoft.Kinect;

namespace Kinteract.Poses.Helpers
{
    public static class MathConvertor
    {
        public static Vector3D To3DVector(this CameraSpacePoint point)
        {
            return new Vector3D
            {
                X = point.X,
                Y = point.Y,
                Z = 0.0
            };
        }
    }
}
