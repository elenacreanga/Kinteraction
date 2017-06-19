using System.Windows.Media;
using System.Windows.Media.Media3D;
using Kinteraction.Helpers;
using Kinteraction.Shapes;
using SharpGL;

namespace Kinteraction
{
    public class Hands
    {
        private readonly ShapeFactory _shapeFactory;
        internal bool IsLeftHandOpen = true;
        internal bool IsRightHandOpen = true;

        internal Point3D LeftHand;
        internal Point3D RightHand;

        internal float[] TransL = new float[3];
        internal float[] TransR = new float[3];


        public Hands(ShapeFactory shapeFactory)
        {
            _shapeFactory = shapeFactory;
        }

        public void Draw(OpenGL gl)
        {
            TransL = new float[3]
                {(float) LeftHand.X / 10 - 30, -(float) LeftHand.Y / 10 + 20, (float) LeftHand.Z * 30 - 25};
            TransR = new float[3]
                {(float) RightHand.X / 10 - 30, -(float) RightHand.Y / 10 + 20, (float) RightHand.Z * 30 - 25};
            TransL = HandLimit(TransL);
            TransR = HandLimit(TransR);

            //Left Hand
            Color lc;
            if (IsLeftHandOpen)
                lc = Colors.Aqua;
            else
                lc = Colors.Red;
            var leftHand = _shapeFactory.GetShape(Type.Sphere);
            leftHand.Origin = new[] {TransL[0], TransL[1], (double) TransL[2]};
            leftHand.Color = lc;
            leftHand.R = 0.5;
            leftHand.Draw(gl);
            DrawTracker(gl, TransL.ToDoubles(), Colors.Aqua);

            //Right Hand
            Color rc;
            if (IsRightHandOpen)
                rc = Colors.Orchid;
            else
                rc = Colors.Red;
            var rightHand = _shapeFactory.GetShape(Type.Sphere);
            rightHand.Origin = new[] {TransR[0], TransR[1], (double) TransR[2]};
            rightHand.Color = rc;
            rightHand.R = 0.5;
            rightHand.Draw(gl);
            DrawTracker(gl, TransR.ToDoubles(), Colors.Orchid);
        }

        private static float[] HandLimit(float[] hand)
        {
            const float xMax = 10.0f;
            const float xMin = -16.0f;
            const float yMax = 13.0f;
            const float yMin = 0.0f;
            const float zMax = 20.0f;
            const float zMin = 0.0f;
            if (hand[0] > xMax) hand[0] = xMax;
            if (hand[0] < xMin) hand[0] = xMin;
            if (hand[1] > yMax) hand[1] = yMax;
            if (hand[1] < yMin) hand[1] = yMin;
            if (hand[2] > zMax) hand[2] = zMax;
            if (hand[2] < zMin) hand[2] = zMin;
            return hand;
        }

        private void DrawTracker(OpenGL gl, double[] origin, Color color)
        {
            var tracker = _shapeFactory.GetShape(Type.Tracker);
            tracker.Origin = origin;
            tracker.Color = color;
            tracker.Draw(gl);
        }
    }
}