using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Kinteraction.Helpers;
using Kinteraction.Shapes;
using SharpGL;
using Type = Kinteraction.Shapes.Type;

namespace Kinteraction
{
    public class Hands
    {
        internal bool _isLeftHandOpen = true;
        internal bool _isRightHandOpen = true;

        internal Point3D _leftHand;
        internal Point3D _rightHand;

        internal float[] transL = new float[3];
        internal float[] transR = new float[3];
        private readonly ShapeFactory _shapeFactory;


        public Hands(ShapeFactory shapeFactory)
        {
            _shapeFactory = shapeFactory;
        }

        public void Draw(OpenGL gl)
        {
            transL = new float[3]
                {(float) _leftHand.X / 10 - 30, -(float) _leftHand.Y / 10 + 20, (float) _leftHand.Z * 30 - 25};
            transR = new float[3]
                {(float) _rightHand.X / 10 - 30, -(float) _rightHand.Y / 10 + 20, (float) _rightHand.Z * 30 - 25};
            transL = HandLimit(transL);
            transR = HandLimit(transR);

            //Left Hand
            Color lc;
            if (_isLeftHandOpen)
                lc = Colors.Aqua;
            else
                lc = Colors.Red;
            var leftHand = _shapeFactory.GetShape(Type.Sphere);
            leftHand.Origin = new[] {transL[0], transL[1], (double) transL[2]};
            leftHand.Color = lc;
            leftHand.R = 0.5;
            leftHand.Draw(gl);
            DrawTracker(gl, transL.ToDoubles(), Colors.Aqua);

            //Right Hand
            Color rc;
            if (_isRightHandOpen)
                rc = Colors.Orchid;
            else
                rc = Colors.Red;
            var rightHand = _shapeFactory.GetShape(Type.Sphere);
            rightHand.Origin = new[] {transR[0], transR[1], (double) transR[2]};
            rightHand.Color = rc;
            rightHand.R = 0.5;
            rightHand.Draw(gl);
            DrawTracker(gl, transR.ToDoubles(), Colors.Orchid);
        }

        private static float[] HandLimit(float[] hand)
        {
            var Xmax = 10.0f;
            var Xmin = -16.0f;
            var Ymax = 13.0f;
            var Ymin = 0.0f;
            var Zmax = 20.0f;
            var Zmin = 0.0f;
            if (hand[0] > Xmax) hand[0] = Xmax;
            if (hand[0] < Xmin) hand[0] = Xmin;
            if (hand[1] > Ymax) hand[1] = Ymax;
            if (hand[1] < Ymin) hand[1] = Ymin;
            if (hand[2] > Zmax) hand[2] = Zmax;
            if (hand[2] < Zmin) hand[2] = Zmin;
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
