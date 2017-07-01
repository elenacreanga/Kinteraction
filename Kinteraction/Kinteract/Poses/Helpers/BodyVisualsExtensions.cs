using System.Collections.Generic;
using Microsoft.Kinect;

namespace Kinteract.Poses.Helpers
{
    public static class BodyVisualsExtensions
    {
        private static IList<Body> _bodies;

        public static IEnumerable<Body> Bodies(this BodyFrame joint)
        {
            if (_bodies == null)
            {
                _bodies = new Body[joint.BodyFrameSource.BodyCount];
            }

            joint.GetAndRefreshBodyData(_bodies);

            return _bodies;
        }
    }
}
