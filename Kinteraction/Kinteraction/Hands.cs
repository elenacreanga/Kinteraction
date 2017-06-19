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
        public Hand Left;
        public Hand Right;

        internal Point3D LeftHand;
        internal Point3D RightHand;



        public Hands(ShapeFactory shapeFactory)
        {
            _shapeFactory = shapeFactory;
            Left = new Hand();
            Right = new Hand();
        }

        public void Draw(OpenGL gl)
        {
            AdjustOrigins();

            //Left Hand
            DrawHand(gl, Colors.Aqua, Left);

            //Right Hand
            DrawHand(gl, Colors.Orchid, Right);
        }

        private void AdjustOrigins()
        {
            Left.Origin = new float[3]
                {(float) LeftHand.X / 10 - 30, -(float) LeftHand.Y / 10 + 20, (float) LeftHand.Z * 30 - 25};
            Right.Origin = new float[3]
                {(float) RightHand.X / 10 - 30, -(float) RightHand.Y / 10 + 20, (float) RightHand.Z * 30 - 25};
            Left.Origin = HandLimit(Left.Origin);
            Right.Origin = HandLimit(Right.Origin);
        }

        private void DrawHand(OpenGL gl, Color handColor, Hand hand)
        {
            var currentColor = hand.IsOpen ? handColor : Colors.Red;
            var shape = _shapeFactory.GetShape(Type.Sphere);
            shape.Origin = hand.Origin.ToDoubles();
            shape.Color = currentColor;
            shape.R = 0.5;
            shape.Draw(gl);
            DrawTracker(gl, hand.Origin.ToDoubles(), handColor);
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