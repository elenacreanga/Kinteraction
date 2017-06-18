﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Kinteraction.Shapes;
using Microsoft.Kinect;
using SharpGL;
using SharpGL.SceneGraph;
using Type = Kinteraction.Shapes.Type;

namespace Kinteraction
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const float InferredZPositionClamp = 0.1f;

        private const float Multipier = 10;
        private readonly BodyFrameReader _bodyFrameReader;
        private readonly WriteableBitmap _colorBitmap;
        private readonly ColorFrameReader _colorFrameReader;
        private readonly CoordinateMapper _coordinateMapper;

        private readonly Hands _hands;
        private readonly KinectSensor _kinectSensor;
        internal readonly ShapeFactory _shapeFactory;
        private readonly IList<Shape> _shapes;
        private double[] _angle;
        private Body[] _bodies;
        private string _detectedText = Constants.NotDetected;
        private float _dist;
        private string _handText = Constants.HandPosition;

        private Mod _mod = Mod.FREE;
        private string _modText = Constants.HandStatus;

        public MainWindow()
        {
            _kinectSensor = KinectSensor.GetDefault();
            _colorFrameReader = _kinectSensor.ColorFrameSource.OpenReader();
            _colorFrameReader.FrameArrived += Reader_ColorFrameArrived;
            var colorFrameDescription = _kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bayer);
            _colorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0,
                PixelFormats.Bgr32, null);
            _bodyFrameReader = _kinectSensor.BodyFrameSource.OpenReader();
            _coordinateMapper = _kinectSensor.CoordinateMapper;
            var frameDescription = _kinectSensor.DepthFrameSource.FrameDescription;
            if (_bodyFrameReader != null)
                _bodyFrameReader.FrameArrived += Reader_FrameArrived;
            _kinectSensor.Open();

            DataContext = this;

            _shapeFactory = new ShapeFactory();
            _shapes = new List<Shape>
            {
                _shapeFactory.GetShape(Type.Cube),
                _shapeFactory.GetShape(Type.Sphere),
                _shapeFactory.GetShape(Type.Pyramid)
            };
            InitializeComponent();
            _hands = new Hands(_shapeFactory);
        }

        public ImageSource ImageSource => _colorBitmap;

        public string DetectedText
        {
            get { return _detectedText; }
            set
            {
                if (_detectedText != value)
                {
                    _detectedText = value;

                    // notify any bound elements that the text has changed
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("DetectedText"));
                }
            }
        }

        public string HandText
        {
            get { return _handText; }
            set
            {
                if (_handText != value)
                {
                    _handText = value;

                    // notify any bound elements that the text has changed
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("HandText"));
                }
            }
        }

        public string ModText
        {
            get => _modText;
            set
            {
                _modText = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ModText"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            var dataReceived = false;
            using (var bodyFrame = e.FrameReference.AcquireFrame())
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
                        var CLPos = body.Joints[JointType.HandLeft].Position;
                        var CRPos = body.Joints[JointType.HandRight].Position;
                        if (CLPos.Z < 0)
                            CLPos.Z = InferredZPositionClamp;
                        if (CRPos.Z < 0)
                            CRPos.Z = InferredZPositionClamp;
                        var DLPos = _coordinateMapper.MapCameraPointToDepthSpace(CLPos);
                        var DRPos = _coordinateMapper.MapCameraPointToDepthSpace(CRPos);
                        _hands._leftHand = new Point3D(DLPos.X, DLPos.Y, CLPos.Z);
                        _hands._rightHand = new Point3D(DRPos.X, DRPos.Y, CRPos.Z);

                        if (body.HandLeftState == HandState.Open || body.HandLeftState == HandState.Closed)
                            _hands._isLeftHandOpen = body.HandLeftState == HandState.Open ? true : false;
                        if (body.HandRightState == HandState.Open || body.HandRightState == HandState.Closed)
                            _hands._isRightHandOpen = body.HandRightState == HandState.Open ? true : false;

                        DetectedText = body.IsTracked ? Constants.Detected : Constants.NotDetected;
                        HandText = "(" + (int) _hands._leftHand.X + "," + (int) _hands._leftHand.Y + "," +
                                   (int) _hands._leftHand.Z +
                                   ")\t(" +
                                   (int) _hands._rightHand.X + "," + (int) _hands._rightHand.Y + "," +
                                   (int) _hands._rightHand.Z + ")";
                    }
        }

        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            // ColorFrame is IDisposable
            using (var colorFrame = e.FrameReference.AcquireFrame())
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
                if (s.Grabbed)
                {
                    for (var k = 0; k < 3; k++) s.Origin[k] = _hands.transR[k];
                    IsZoom(s);
                    if (_mod == Mod.ZOOM)
                    {
                        Console.WriteLine(s.R + " " + _dist + " " + Euclid(_hands.transL, _hands.transR));
                        s.Rp = (Euclid(_hands.transL, _hands.transR) - _dist) * 0.3;
                        s.Rop = new double[3]
                        {
                            _hands.transL[0] - _hands.transR[0] - _angle[0],
                            _hands.transL[1] - _hands.transR[1] - _angle[1],
                            _hands.transL[2] - _hands.transR[2] - _angle[2]
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
                if (!_hands._isRightHandOpen)
                    if (!s.Grabbed)
                        if (s.Origin[0] - s.R < _hands.transR[0] && s.Origin[0] + s.R > _hands.transR[0] &&
                            s.Origin[1] - s.R < _hands.transR[1] && s.Origin[1] + s.R > _hands.transR[1] &&
                            s.Origin[2] - s.R < _hands.transR[2] && s.Origin[2] + s.R > _hands.transR[2])
                        {
                            s.Grabbed = true;
                            _mod = Mod.GRAB;
                        }
            }
            else
            {
                if (_hands._isRightHandOpen)
                    if (s.Grabbed)
                    {
                        s.Grabbed = false;
                        _mod = Mod.FREE;
                    }
            }
        }

        private void IsZoom(Shape s)
        {
            if (_mod == Mod.GRAB)
            {
                if (!_hands._isLeftHandOpen)
                {
                    _mod = Mod.ZOOM;
                    _dist = Euclid(_hands.transL, _hands.transR);
                    _angle = new double[3]
                    {
                        _hands.transL[0] - _hands.transR[0], _hands.transL[1] - _hands.transR[1],
                        _hands.transL[2] - _hands.transR[2]
                    };
                }
            }
            else
            {
                if (_hands._isLeftHandOpen)
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

        private enum Mod
        {
            FREE,
            GRAB,
            ZOOM,
            LASSO
        }
    }
}