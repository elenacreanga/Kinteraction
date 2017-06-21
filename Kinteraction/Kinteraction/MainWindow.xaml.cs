using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Kinteraction.Frames;
using Kinteraction.Helpers;
using Kinteraction.Kinteract.Gestures;
using Kinteraction.Kinteract.Players;
using Kinteraction.Kinteract.Poses.Helpers;
using Kinteraction.Shapes;
using Microsoft.Kinect;
using SharpGL;
using SharpGL.SceneGraph;
using Constants = Kinteraction.Helpers.Constants;
using Type = Kinteraction.Shapes.Type;

namespace Kinteraction
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const float Multipier = 10;
        private GestureFacade _gestureFacade;

        private Hands _hands;
        private readonly KinectSensor _kinectSensor;
        private readonly MultiSourceFrameReader _multiSourceFrameReader;
        private ShapeFactory _shapeFactory;
        private IList<Shape> _shapes;
        private UserFacade _userFacade;
        private HandGestures _handGestures;

        private Body[] _bodies;

        private string _modText = Constants.HandStatus;

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

            InitializeShapes();
        }

        private void InitializeShapes()
        {
            _shapeFactory = new ShapeFactory();
            _shapes = new List<Shape>
            {
                _shapeFactory.GetShape(Type.Cube),
                _shapeFactory.GetShape(Type.Sphere),
                _shapeFactory.GetShape(Type.Pyramid)
            };

            _hands = new Hands(_shapeFactory, _kinectSensor.CoordinateMapper);
            _hands.PropertyChanged += TextProperties_Changed;

            _handGestures = new HandGestures(_hands);
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

        public string ModText
        {
            get => _modText;
            set
            {
                _modText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ModText"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void TextProperties_Changed(object sender, EventArgs e)
        {
            var hands = (Hands) sender;
            DetectedText.Text = hands.DetectedText;
            HandText.Text = hands.HandText;
        }

        private void Reader_FrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var dataReceived = false;
            var reference = e.FrameReference.AcquireFrame();

            RenderImage(reference);


            using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (_bodies == null)
                        _bodies = new Body[bodyFrame.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(_bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
                foreach (var body in _bodies)
                    if (body.IsTracked)
                    {
                        _hands.UpdateHandsState(body);
                        _gestureFacade.Update(body);
                    }
        }

        private void RenderImage(MultiSourceFrame reference)
        {
            RenderColorFrame(reference);
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    var bodies = frame.Bodies();
                    _userFacade.Update(bodies);
                    foreach (var body in bodies)
                        viewer.DrawBody(body);
                }
            }
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
            InitGl(gl);
            DrawAxis(gl);

            _hands.Draw(gl);
            DrawShapes(gl);
            gl.Flush();
        }

        private void DrawShapes(OpenGL gl)
        {
            foreach (var shape in _shapes)
                DrawItem(gl, shape);
        }

        private static void InitGl(OpenGL gl)
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
            _handGestures.IsGrabbed(s);
            ModText = Enum.GetName(typeof(Mod), _handGestures.Mod);
            Console.WriteLine(_handGestures.Mod);
            if (_handGestures.Mod == Mod.GRAB || _handGestures.Mod == Mod.ZOOM)
                if (s.IsGrabbed)
                {
                    for (var k = 0; k < 3; k++) s.Origin[k] = _hands.Right.Origin[k];
                    _handGestures.IsZoom(s);
                    if (_handGestures.Mod == Mod.ZOOM)
                    {
                        Console.WriteLine(s.R + " " + _handGestures.Dist + " " + Euclid.Calculate(_hands.Left.Origin, _hands.Right.Origin));
                        s.Rp = (Euclid.Calculate(_hands.Left.Origin, _hands.Right.Origin) - _handGestures.Dist) * 0.3;
                        s.Rop = new double[3]
                        {
                            _hands.Left.Origin[0] - _hands.Right.Origin[0] - _handGestures.Angle[0],
                            _hands.Left.Origin[1] - _hands.Right.Origin[1] - _handGestures.Angle[1],
                            _hands.Left.Origin[2] - _hands.Right.Origin[2] - _handGestures.Angle[2]
                        };
                    }
                }
            s.Draw(gl);
            gl.PopMatrix();
        }

        private void DrawAxis(OpenGL gl)
        {
            var axis = _shapeFactory.GetShape(Type.Axis);
            axis.Draw(gl);
        }
        
        private void OpenGLControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void GestureFacade_GestureRecognized(object sender, GestureEventArgs e)
        {
            if (e.Type == Kinteract.Gestures.Type.WaveRight)
                if (_handGestures.Mod == Mod.FREE)
                    _shapes.Clear();
        }

        private void UserReporterUserEntered(object sender, UsersFacadeEventArgs e)
        {
        }

        private void UserReporterUserLeft(object sender, UsersFacadeEventArgs e)
        {
            viewer.Clear();
        }
    }
}