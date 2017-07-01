using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Kinteraction.Helpers;
using Microsoft.Kinect;
using SharpGL;

namespace Kinteraction.ShapeModelling.Shapes
{
    public class Hands
    {
        private const float InferredZPositionClamp = 0.1f;
        private readonly CoordinateMapper _coordinateMapper;
        private string _detectedText = Constants.NotDetected;
        private string _handText = Constants.HandPosition;
        public Hand Left;
        public Hand Right;

        public Hands(ShapeFactory shapeFactory, CoordinateMapper coordinateMapper)
        {
            Left = new Hand(shapeFactory);
            Right = new Hand(shapeFactory);
            _coordinateMapper = coordinateMapper;
        }

        public string HandText
        {
            get => _handText;
            set
            {
                if (_handText == value) return;
                _handText = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HandText"));
            }
        }

        public string DetectedText
        {
            get => _detectedText;
            set
            {
                if (_detectedText == value) return;
                _detectedText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DetectedText"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Draw(OpenGL gl)
        {
            DrawLeft(gl);
            DrawRight(gl);
        }

        private void DrawRight(OpenGL gl)
        {
            Hand.AdjustOrigin(Right);
            Right.Draw(gl, Colors.Orchid, Right);
        }

        private void DrawLeft(OpenGL gl)
        {
            Hand.AdjustOrigin(Left);
            Left.Draw(gl, Colors.Aqua, Left);
        }

        public void UpdateHandsState(Body body)
        {
            UpdateHandState(body, JointType.HandLeft, Left, body.HandLeftState);

            UpdateHandState(body, JointType.HandRight, Right, body.HandRightState);

            UpdateText(body);
        }

        private void UpdateHandState(Body body, JointType jointType, Hand hand, HandState bodyHandState)
        {
            var handPosition = body.Joints[jointType].Position;
            if (handPosition.Z < 0)
                handPosition.Z = InferredZPositionClamp;
            var depthHandPosition = _coordinateMapper.MapCameraPointToDepthSpace(handPosition);
            hand.Point3D = new Point3D(depthHandPosition.X, depthHandPosition.Y,
                handPosition.Z);
            if (bodyHandState == HandState.Open || bodyHandState == HandState.Closed)
                hand.IsOpen = bodyHandState == HandState.Open;
        }

        private void UpdateText(Body body)
        {
            DetectedText = body.IsTracked ? Constants.Detected : Constants.NotDetected;
            var leftHandPosition = FormatHandPosition(Left.Point3D);
            var rightHandPosition = FormatHandPosition(Right.Point3D);
            HandText = leftHandPosition + "\t" + rightHandPosition;
        }

        private static string FormatHandPosition(Point3D point3D)
        {
            var x = (int) point3D.X;
            var y = (int) point3D.Y;
            var z = (int) point3D.Z;

            var handPosition = $"({x}, {y}, {z}) ";
            return handPosition;
        }
    }
}