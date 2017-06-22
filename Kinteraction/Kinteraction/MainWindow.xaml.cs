using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Kinteraction.Annotations;
using Kinteraction.Frames;
using Kinteraction.Helpers;
using Kinteraction.Kinteract.Gestures;
using Kinteraction.Kinteract.Players;
using Kinteraction.Kinteract.Poses.Helpers;
using Kinteraction.Shapes;
using Microsoft.Kinect;
using SharpGL;
using SharpGL.SceneGraph;
using Type = Kinteraction.Shapes.Type;

namespace Kinteraction
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly KinectSensor _kinectSensor;
        private readonly MultiSourceFrameReader _multiSourceFrameReader;

        private Body[] _bodies;
        private GestureFacade _gestureFacade;
        private ShapeFactory _shapeFactory;
        private UserFacade _userFacade;
        private DrawingBoard _drawingBoard;

        public MainWindow()
        {
            _kinectSensor = KinectSensor.GetDefault();
            _multiSourceFrameReader =
                _kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth |
                                                         FrameSourceTypes.Infrared | FrameSourceTypes.Body);
            if (_multiSourceFrameReader != null)
                _multiSourceFrameReader.MultiSourceFrameArrived += Reader_FrameArrived;
            _kinectSensor.Open();

            DataContext = this;


            InitializeComponent();

            InitializeFacades();

            InitializeDrawingBoard();
        }

        private void InitializeDrawingBoard()
        {
            _shapeFactory = new ShapeFactory();
           
            _drawingBoard = new DrawingBoard(_shapeFactory, _kinectSensor.CoordinateMapper);
            _drawingBoard.PropertyChanged += HandProperties_Changed;
        }

        private void InitializeFacades()
        {
            _gestureFacade = new GestureFacade();
            _gestureFacade.GestureRecognized += GestureFacade_GestureRecognized;

            _userFacade = new UserFacade();
            _userFacade.UserEntered += UserReporterUserEntered;
            _userFacade.UserLeft += UserReporterUserLeft;
            _userFacade.Start();
        }

        private void HandProperties_Changed(object sender, EventArgs e)
        {
            var drawingBoard = (DrawingBoard) sender;
            DetectedText.Text = drawingBoard.Hands.DetectedText;
            HandText.Text = drawingBoard.Hands.HandText;
            ModText.Text = drawingBoard.HandGestures.ModText;
        }

        private void Reader_FrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            var dataReceived = RenderImage(reference);

            if (dataReceived)
            {
                foreach (var body in _bodies)
                    if (body.IsTracked)
                    {
                        _drawingBoard.UpdateHandsState(body);
                        _gestureFacade.Update(body);
                    }
            }
        }

        private bool RenderImage(MultiSourceFrame reference)
        {
            RenderColorFrame(reference);
            return RenderBodyFrame(reference);
        }

        private bool RenderBodyFrame(MultiSourceFrame reference)
        {
            var dataReceived = false;
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    var bodies = frame.Bodies();
                    if (_bodies == null)
                        _bodies = bodies.ToArray();

                    _userFacade.Update(bodies);
                    foreach (var body in bodies)
                    { viewer.DrawBody(body);}
                    dataReceived = true;
                }
            }
            return dataReceived;
        }

        private void RenderColorFrame(MultiSourceFrame reference)
        {
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame == null) return;
                if (viewer.Visualization == Visualization.Color)
                    viewer.ImageSource = frame.ToBitmap();
            }
        }

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            var gl = args.OpenGL;
            _drawingBoard.Initialize(gl);

            gl.Flush();
        }

        private void OpenGLControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void GestureFacade_GestureRecognized(object sender, GestureEventArgs e)
        {
            if (e.Type == Kinteract.Gestures.Type.WaveRight)
            {
                _drawingBoard.Clear();
            }
        }

        private void UserReporterUserEntered(object sender, UsersFacadeEventArgs e)
        {
        }

        private void UserReporterUserLeft(object sender, UsersFacadeEventArgs e)
        {
            viewer.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}