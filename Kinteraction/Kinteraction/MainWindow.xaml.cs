using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Kinteraction.Kinteract.Gestures;
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
        private readonly WriteableBitmap _colorBitmap;

        private readonly Hands _hands;
        private readonly KinectSensor _kinectSensor;
        private readonly MultiSourceFrameReader _multiSourceFrameReader;
        private readonly ShapeFactory _shapeFactory;
        private readonly IList<Shape> _shapes;
        private double[] _angle;
        private Body[] _bodies;
        private float _dist;
        private readonly GestureFacade _gestureFacade;

        private Mod _mod = Mod.FREE;
        private string _modText = Constants.HandStatus;

        public MainWindow()
        {
            _kinectSensor = KinectSensor.GetDefault();
            var colorFrameDescription = _kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bayer);
            _colorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0,
                PixelFormats.Bgr32, null);
            _multiSourceFrameReader =
                _kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth |
                                                         FrameSourceTypes.Infrared | FrameSourceTypes.Body);
            if (_multiSourceFrameReader != null)
                _multiSourceFrameReader.MultiSourceFrameArrived += Reader_FrameArrived;
            _kinectSensor.Open();

            _gestureFacade = new GestureFacade();
            _gestureFacade.GestureRecognized += GestureFacade_GestureRecognized;

            DataContext = this;

            _shapeFactory = new ShapeFactory();
            _shapes = new List<Shape>
            {
                _shapeFactory.GetShape(Type.Cube),
                _shapeFactory.GetShape(Type.Sphere),
                _shapeFactory.GetShape(Type.Pyramid)
            };
            InitializeComponent();
            _hands = new Hands(_shapeFactory, _kinectSensor.CoordinateMapper);
            _hands.PropertyChanged += TextProperties_Changed;
        }

        public ImageSource ImageSource => _colorBitmap;

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
            using (var colorFrame = reference.ColorFrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    var colorFrameDescription = colorFrame.FrameDescription;

                    using (var colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        _colorBitmap.Lock();

                        // verify data and write the new color frame data to the display bitmap
                        if (colorFrameDescription.Width == _colorBitmap.PixelWidth &&
                            colorFrameDescription.Height == _colorBitmap.PixelHeight)
                        {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                _colorBitmap.BackBuffer,
                                (uint) (colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            _colorBitmap.AddDirtyRect(
                                new Int32Rect(0, 0, _colorBitmap.PixelWidth, _colorBitmap.PixelHeight));
                        }

                        _colorBitmap.Unlock();
                    }
                }
            }
        }

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            var gl = args.OpenGL;
            initGL(gl);
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

        private void initGL(OpenGL gl)
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
            IsGrabbed(s);
            ModText = Enum.GetName(typeof(Mod), _mod);
            Console.WriteLine(_mod);
            if (_mod == Mod.GRAB || _mod == Mod.ZOOM)
                if (s.IsGrabbed)
                {
                    for (var k = 0; k < 3; k++) s.Origin[k] = _hands.Right.Origin[k];
                    IsZoom(s);
                    if (_mod == Mod.ZOOM)
                    {
                        Console.WriteLine(s.R + " " + _dist + " " + Euclid(_hands.Left.Origin, _hands.Right.Origin));
                        s.Rp = (Euclid(_hands.Left.Origin, _hands.Right.Origin) - _dist) * 0.3;
                        s.Rop = new double[3]
                        {
                            _hands.Left.Origin[0] - _hands.Right.Origin[0] - _angle[0],
                            _hands.Left.Origin[1] - _hands.Right.Origin[1] - _angle[1],
                            _hands.Left.Origin[2] - _hands.Right.Origin[2] - _angle[2]
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

        private void IsGrabbed(Shape s)
        {
            if (_mod == Mod.FREE)
            {
                if (!_hands.Right.IsOpen)
                    if (!s.IsGrabbed)
                        if (s.Origin[0] - s.R < _hands.Right.Origin[0] && s.Origin[0] + s.R > _hands.Right.Origin[0] &&
                            s.Origin[1] - s.R < _hands.Right.Origin[1] && s.Origin[1] + s.R > _hands.Right.Origin[1] &&
                            s.Origin[2] - s.R < _hands.Right.Origin[2] && s.Origin[2] + s.R > _hands.Right.Origin[2])
                        {
                            s.IsGrabbed = true;
                            _mod = Mod.GRAB;
                        }
            }
            else
            {
                if (_hands.Right.IsOpen)
                    if (s.IsGrabbed)
                    {
                        s.IsGrabbed = false;
                        _mod = Mod.FREE;
                    }
            }
        }

        private void IsZoom(Shape s)
        {
            if (_mod == Mod.GRAB)
            {
                if (!_hands.Left.IsOpen)
                {
                    _mod = Mod.ZOOM;
                    _dist = Euclid(_hands.Left.Origin, _hands.Right.Origin);
                    _angle = new double[3]
                    {
                        _hands.Left.Origin[0] - _hands.Right.Origin[0], _hands.Left.Origin[1] - _hands.Right.Origin[1],
                        _hands.Left.Origin[2] - _hands.Right.Origin[2]
                    };
                }
            }
            else
            {
                if (_hands.Left.IsOpen)
                {
                    _mod = Mod.GRAB;
                    s.R += s.Rp;
                    s.Rp = 0;
                    for (var k = 0; k < 3; k++)
                    {
                        s.Ro[k] += s.Rop[k];
                        s.Rop[k] = 0;
                    }
                    if (s.R < 1) s.R = 1;
                }
            }
        }

        private static float Euclid(float[] a, float[] b)
        {
            return (float) Math.Sqrt((a[0] - b[0]) * (a[0] - b[0]) + (a[1] - b[1]) * (a[1] - b[1]) +
                                     (a[2] - b[2]) * (a[2] - b[2]));
        }

        private void OpenGLControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void GestureFacade_GestureRecognized(object sender, GestureEventArgs e)
        {
            if (e.Type == Kinteract.Gestures.Type.WaveRight)
                if (_mod == Mod.FREE)
                    _shapes.Clear();
        }

        private enum Mod
        {
            FREE,
            GRAB,
            ZOOM,
            LASSO
        }
    }
}