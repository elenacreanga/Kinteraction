using System.Windows.Media;
using System.Windows.Media.Media3D;
using Kinteraction.Helpers;
using SharpGL;

namespace Kinteraction.Shapes
{
    public class Hand
    {
        private readonly ShapeFactory _shapeFactory;

        public Hand(ShapeFactory shapeFactory)
        {
            _shapeFactory = shapeFactory;
            IsOpen = true;
            Origin = new float[3];
        }

        public bool IsOpen { get; set; }
        public float[] Origin { get; set; }

        public Point3D Point3D { get; set; }

        public void Draw(OpenGL gl, Color handColor, Hand hand)
        {
            var currentColor = hand.IsOpen ? handColor : Colors.Red;
            var shape = _shapeFactory.GetShape(Type.Sphere);
            shape.Origin = hand.Origin.ToDoubles();
            shape.Color = currentColor;
            shape.R = 0.5;
            shape.Draw(gl);
            DrawTracker(gl, hand.Origin.ToDoubles(), handColor);
        }

        internal static void AdjustOrigin(Hand hand)
        {
            hand.Origin = new float[3]
                {(float) hand.Point3D.X / 10 - 30, -(float) hand.Point3D.Y / 10 + 20, (float) hand.Point3D.Z * 30 - 25};

            hand.Origin = HandLimit(hand.Origin);
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