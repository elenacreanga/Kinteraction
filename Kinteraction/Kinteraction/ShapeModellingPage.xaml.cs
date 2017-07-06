using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Kinteract.Gestures;
using Kinteract.Players;
using Kinteract.Poses.Helpers;
using Kinteraction.Frames;
using Kinteraction.Helpers;
using Kinteraction.Properties;
using Kinteraction.ShapeModelling;
using Kinteraction.ShapeModelling.Shapes;
using Microsoft.Kinect;
using SharpGL.SceneGraph;
using Type = Kinteract.Gestures.Type;

namespace Kinteraction
{
    public partial class ShapeModellingPage : Page, INotifyPropertyChanged
    {
        private readonly KinectSensor _kinectSensor;

        private Body[] _bodies;
        private DrawingBoard _drawingBoard;
        private GestureFacade _gestureFacade;
        private ShapeFactory _shapeFactory;
        private UserFacade _userFacade;

        public ShapeModellingPage()
        {
            _kinectSensor = KinectSensor.GetDefault();
            var multiSourceFrameReader = _kinectSensor.OpenMultiSourceFrameReader(
                FrameSourceTypes.Color | FrameSourceTypes.Depth |
                FrameSourceTypes.Infrared | FrameSourceTypes.Body);
            if (multiSourceFrameReader != null)
                multiSourceFrameReader.MultiSourceFrameArrived += Reader_FrameArrived;
            _kinectSensor.Open();

            DataContext = this;


            InitializeComponent();

            InitializeFacades();

            InitializeDrawingBoard();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
            //RenderDepthFrame(reference);
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
                    {
                        viewer.DrawBody(body);
                    }
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
        //private void RenderDepthFrame(MultiSourceFrame reference)
        //{
        //    using (var frame = reference.DepthFrameReference.AcquireFrame())
        //    {
        //        if (frame == null) return;
        //        if (viewer.Visualization == Visualization.Depth)
        //        {
        //            viewer.ImageSource = frame.ToBitmap();
        //        }
        //    }
        //}

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
            switch (e.Type)
            {
                case Type.WaveRight:
                    _drawingBoard.Undo();
                    break;
                case Type.Clap:
                    _drawingBoard.Reset();
                    break;
                case Type.SwipeRight:
                    break;
                case Type.SwipeLeft:
                    break;
                case Type.WaveLeft:
                    break;
                case Type.ZoomIn:
                    break;
                case Type.ZoomOut:
                    break;
                case Type.CrossedArms:
                    _drawingBoard.Clear();
                    break;
                case Type.Surrender:
                    break;
                case Type.KickRight:
                    break;
                case Type.KickLeft:
                    break;
                case Type.LiftRightLeg:
                    break;
                case Type.LiftLeftLeg:
                    break;
                case Type.Jump:
                    break;
                case Type.Squat:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UserReporterUserEntered(object sender, UsersFacadeEventArgs e)
        {
        }

        private void UserReporterUserLeft(object sender, UsersFacadeEventArgs e)
        {
            viewer.Clear();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}