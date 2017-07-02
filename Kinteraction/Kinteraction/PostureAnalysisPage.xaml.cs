using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Kinteract.Players;
using Kinteract.Poses.Helpers;
using Kinteraction.Frames;
using Kinteraction.Helpers;
using Microsoft.Kinect;

namespace Kinteraction
{
    public partial class PostureAnalysisPage : Page
    {
        private readonly KinectSensor _kinectSensor;
        private UserFacade _userFacade;

        private Body[] _bodies;
        public PostureAnalysisPage()
        {
            InitializeComponent();
            _kinectSensor = KinectSensor.GetDefault();
            var multiSourceFrameReader = _kinectSensor.OpenMultiSourceFrameReader(
                FrameSourceTypes.Color | FrameSourceTypes.Depth |
                FrameSourceTypes.Infrared | FrameSourceTypes.Body);
            if (multiSourceFrameReader != null)
                multiSourceFrameReader.MultiSourceFrameArrived += Reader_FrameArrived;
            _kinectSensor.Open();
            _userFacade = new UserFacade();
        }

        private void Reader_FrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            var dataReceived = RenderImage(reference);
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
