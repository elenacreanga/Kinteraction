using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kinteraction.Annotations;
using Kinteraction.Helpers;
using Kinteraction.Shapes;
using Microsoft.Kinect;
using SharpGL;
using Type = Kinteraction.Shapes.Type;

namespace Kinteraction
{
    internal class DrawingBoard : INotifyPropertyChanged
    {
        private const float Multipier = 10;

        private HandGestures _handGestures;

        public HandGestures HandGestures
        {
            get => _handGestures;
            set { _handGestures = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text")); }
        }
        public Hands Hands {
            get => _hands;
            set { _hands = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text")); } }
        private readonly ShapeFactory _shapeFactory;
        private readonly IList<Shape> _shapes;
        private Hands _hands;

        public DrawingBoard(ShapeFactory shapeFactory, CoordinateMapper coordinateMapper)
        {
            _shapeFactory = shapeFactory;
            _shapes = new List<Shape>
            {
                _shapeFactory.GetShape(Type.Cube),
                _shapeFactory.GetShape(Type.Sphere),
                _shapeFactory.GetShape(Type.Pyramid)
            };
            Hands = new Hands(_shapeFactory, coordinateMapper);
            Hands.PropertyChanged += TextProperties_Changed;

            HandGestures = new HandGestures(_hands);
            HandGestures.PropertyChanged += TextProperties_Changed;
        }

        internal void DrawShapes(OpenGL gl)
        {
            foreach (var shape in _shapes)
                DrawItem(gl, shape);
        }

        public void Initialize(OpenGL gl)
        {
            ResetBoard(gl);
            DrawAxis(gl);
            DrawShapes(gl);
            Hands.Draw(gl);
        }

        private static void ResetBoard(OpenGL gl)
        {
            //  Clear the color and depth buffers.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Reset the modelview.
            gl.LoadIdentity();
            gl.LookAt(-20, 20, 40, 0, 0, 0, 0, 1, 0);
            //  Move into a more central position.
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
        }

        private void DrawItem(OpenGL gl, Shape s)
        {
            gl.PushMatrix();
            gl.Translate(s.Origin[0], s.Origin[1], s.Origin[2]);
            gl.Rotate((s.Ro[1] + s.Rop[1]) * Multipier, 1, 0, 0);
            gl.Rotate((s.Ro[2] + s.Rop[2]) * Multipier, 0, 1, 0);
            gl.Rotate((s.Ro[0] + s.Rop[0]) * Multipier, 0, 0, 1);
            HandGestures.IsGrabbed(s);

            if (HandGestures.Mod == Mod.GRAB || HandGestures.Mod == Mod.ZOOM)
                if (s.IsGrabbed)
                {
                    for (var k = 0; k < 3; k++) s.Origin[k] = Hands.Right.Origin[k];
                    HandGestures.IsZoom(s);
                    if (HandGestures.Mod == Mod.ZOOM)
                    {
                        Console.WriteLine(s.R + " " + HandGestures.Dist + " " +
                                          Euclid.Calculate(Hands.Left.Origin, Hands.Right.Origin));
                        s.Rp = (Euclid.Calculate(Hands.Left.Origin, Hands.Right.Origin) - HandGestures.Dist) * 0.3;
                        s.Rop = new double[3]
                        {
                            Hands.Left.Origin[0] - Hands.Right.Origin[0] - HandGestures.Angle[0],
                            Hands.Left.Origin[1] - Hands.Right.Origin[1] - HandGestures.Angle[1],
                            Hands.Left.Origin[2] - Hands.Right.Origin[2] - HandGestures.Angle[2]
                        };
                    }
                }
            s.Draw(gl);
            gl.PopMatrix();
        }

        public void DrawAxis(OpenGL gl)
        {
            var axis = _shapeFactory.GetShape(Type.Axis);
            axis.Draw(gl);
        }

        public void Clear()
        {
            _shapes.Clear();
        }

        public void UpdateHandsState(Body body)
        {
            Hands.UpdateHandsState(body);
        }

        private void TextProperties_Changed(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Hands));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}