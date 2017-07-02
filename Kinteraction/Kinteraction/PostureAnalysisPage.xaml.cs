using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using Microsoft.Kinect;
using Type = Kinteract.Gestures.Type;

namespace Kinteraction
{
    public partial class PostureAnalysisPage : Page
    {
        private readonly Stopwatch _stopwatch;
        private UserFacade _userFacade;

        private Body[] _bodies;
        private DateTime _dateTime;
        private readonly string _fileName;
        private readonly string _user;
        private GestureFacade _gestureFacade;
        public PostureAnalysisPage()
        {
            InitializeComponent();
            var kinectSensor = KinectSensor.GetDefault();
            var multiSourceFrameReader = kinectSensor.OpenMultiSourceFrameReader(
                FrameSourceTypes.Color | FrameSourceTypes.Depth |
                FrameSourceTypes.Infrared | FrameSourceTypes.Body);
            if (multiSourceFrameReader != null)
                multiSourceFrameReader.MultiSourceFrameArrived += Reader_FrameArrived;
            kinectSensor.Open();
            InitializeFacades();
            _dateTime = DateTime.Now;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            _fileName = "DressingRoom";
            _user = "U_" + new Random().Next();
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

        private void Reader_FrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            var dataReceived = RenderImage(reference);
            if (dataReceived)
            {
                foreach (var body in _bodies)
                    if (body.IsTracked)
                    {
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
                        viewer.DrawBody(body);
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

        private void WriteToFile(Type gesture)
        {
            var stopwatch = _stopwatch.Elapsed;
            var text = _user + ";" + stopwatch + ";" + (int)gesture + ";" + gesture;
            File.AppendAllLines(_fileName, new[]{text});
        }

        private void GestureFacade_GestureRecognized(object sender, GestureEventArgs e)
        {
            switch (e.Type)
            {
                case Type.WaveRight:
                    Posture.Text = "WaveRight detected";
                    WriteToFile(e.Type);
                    break;
                case Type.Clap:
                    Posture.Text = "Clap detected";
                    WriteToFile(e.Type);
                    break;
                case Type.SwipeRight:
                    Posture.Text = "SwipeRight detected";
                    WriteToFile(e.Type);
                    break;
                case Type.SwipeLeft:
                    Posture.Text = "SwipeLeft detected";
                    WriteToFile(e.Type);
                    break;
                case Type.WaveLeft:
                    Posture.Text = "WaveLeft detected";
                    WriteToFile(e.Type);
                    break;
                case Type.ZoomIn:
                    Posture.Text = "ZoomIn detected";
                    WriteToFile(e.Type);
                    break;
                case Type.ZoomOut:
                    Posture.Text = "ZoomOut detected";
                    WriteToFile(e.Type);
                    break;
                case Type.CrossedArms:
                    Posture.Text = "Crossed Arms detected";
                    WriteToFile(e.Type);
                    break;
                case Type.Surrender:
                    Posture.Text = "Surrender detected";
                    WriteToFile(e.Type);
                    break;
                case Type.Squat:
                    Posture.Text = "Squat detected";
                    WriteToFile(e.Type);
                    break;
                case Type.Jump:
                    Posture.Text = "Jump detected";
                    WriteToFile(e.Type);
                    break;
                case Type.KickLeft:
                    Posture.Text = "KickLeft detected";
                    WriteToFile(e.Type);
                    break;
                case Type.KickRight:
                    Posture.Text = "KickRight detected";
                    WriteToFile(e.Type);
                    break;
                case Type.LiftRightLeg:
                    Posture.Text = "LiftRightLeg detected";
                    WriteToFile(e.Type);
                    break;
                case Type.LiftLeftLeg:
                    Posture.Text = "LiftLeftLeg detected";
                    WriteToFile(e.Type);
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}