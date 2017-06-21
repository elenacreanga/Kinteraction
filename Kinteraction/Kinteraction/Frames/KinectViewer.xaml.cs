using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Kinect;

namespace Kinteraction.Frames
{
    public partial class KinectViewer : UserControl
    {
        private static readonly double DEFAULT_RADIUS = 15;
        private static readonly double DEFAULT_THICKNESS = 8;

        public static readonly DependencyProperty CoordinateMapperProperty =
            DependencyProperty.Register("CoordinateMapper", typeof(CoordinateMapper), typeof(KinectViewer),
                new FrameworkPropertyMetadata(KinectSensor.GetDefault().CoordinateMapper));

        public static readonly DependencyProperty VisualizationProperty =
            DependencyProperty.Register("Visualization", typeof(Visualization), typeof(KinectViewer),
                new FrameworkPropertyMetadata(Visualization.Color));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(KinectViewer),
                new FrameworkPropertyMetadata(null));

        private readonly List<BodyVisual> _bodyVisuals = new List<BodyVisual>();

        public readonly IDictionary<BrushType, Brush> BrushTypes = new Dictionary<BrushType, Brush>
        {
            {BrushType.LightCyan, new SolidColorBrush(Colors.LightCyan)},
            {BrushType.LightCoral, new SolidColorBrush(Colors.LightCoral)},
            {BrushType.LightGreen, new SolidColorBrush(Colors.LightGreen)},
            {BrushType.LightSkyBlue, new SolidColorBrush(Colors.LightSkyBlue)},
            {BrushType.LightPink, new SolidColorBrush(Colors.LightPink)},
            {BrushType.LightSeaGreen, new SolidColorBrush(Colors.LightSeaGreen)}
        };

        public KinectViewer()
        {
            InitializeComponent();

            DataContext = this;
        }

        public CoordinateMapper CoordinateMapper
        {
            get => (CoordinateMapper) GetValue(CoordinateMapperProperty);
            set => SetValue(CoordinateMapperProperty, value);
        }

        public Visualization Visualization
        {
            get => (Visualization) GetValue(VisualizationProperty);
            set => SetValue(VisualizationProperty, value);
        }

        public ImageSource ImageSource
        {
            get => (ImageSource) GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public void Clear()
        {
            canvas.Children.Clear();

            foreach (var visual in _bodyVisuals)
                visual.Clear();

            _bodyVisuals.Clear();
        }

        public Point GetPoint2D(CameraSpacePoint position)
        {
            var point = new Point();

            switch (Visualization)
            {
                case Visualization.Color:
                {
                    var colorPoint = CoordinateMapper.MapCameraPointToColorSpace(position);
                    point.X = float.IsInfinity(colorPoint.X) ? 0.0 : colorPoint.X;
                    point.Y = float.IsInfinity(colorPoint.Y) ? 0.0 : colorPoint.Y;
                }
                    break;
                case Visualization.Depth:
                case Visualization.Infrared:
                {
                    var depthPoint = CoordinateMapper.MapCameraPointToDepthSpace(position);
                    point.X = float.IsInfinity(depthPoint.X) ? 0.0 : depthPoint.X;
                    point.Y = float.IsInfinity(depthPoint.Y) ? 0.0 : depthPoint.Y;
                }
                    break;
                default:
                    break;
            }

            return point;
        }

        public void DrawBody(Body body, double jointRadius, Brush jointBrush, double boneThickness, Brush boneBrush)
        {
            if (body == null || !body.IsTracked) return;

            var visual = _bodyVisuals.FirstOrDefault(b => b.TrackingId == body.TrackingId);

            if (visual == null)
            {
                visual = BodyVisual.Create(body.TrackingId, body.Joints.Keys, jointRadius, jointBrush, boneThickness,
                    boneBrush);

                foreach (var ellipse in visual.Joints.Values)
                    canvas.Children.Add(ellipse);

                foreach (var line in visual.Bones.Values)
                    canvas.Children.Add(line);

                _bodyVisuals.Add(visual);
            }

            foreach (var joint in body.Joints)
            {
                var point = GetPoint2D(joint.Value.Position);

                visual.UpdateJoint(joint.Key, point);
            }

            foreach (var bone in visual.Connections)
            {
                var first = GetPoint2D(body.Joints[bone.Item1].Position);
                var second = GetPoint2D(body.Joints[bone.Item2].Position);

                visual.UpdateBone(bone, first, second);
            }
        }

        public void DrawBody(Body body)
        {
            DrawBody(body, DEFAULT_RADIUS, BrushTypes[BrushType.LightCoral], DEFAULT_THICKNESS,
                BrushTypes[BrushType.LightCoral]);
        }
    }

    public enum Visualization
    {
        Color = 0,
        Depth = 1,
        Infrared = 3
    }

    public enum BrushType
    {
        LightCyan = 0,
        LightCoral,
        LightGreen,
        LightSkyBlue,
        LightPink,
        LightSeaGreen
    }
}