using System;
using System.Collections.Generic;
using System.Linq;
using Kinteraction.Kinteract.Gestures.Segments;
using Microsoft.Kinect;

namespace Kinteraction.Kinteract.Gestures
{
    public class GestureFacade : BaseFacade<Body>
    {
        private readonly List<Gesture> _gestures = new List<Gesture>();

        public GestureFacade()
        {
            foreach (Type t in Enum.GetValues(typeof(Type)))
                AddGesture(t);
        }

        public GestureFacade(Type type)
        {
            AddGesture(type);
        }

        public event EventHandler<GestureEventArgs> GestureRecognized;

        public override void Update(Body body)
        {
            base.Update(body);

            foreach (var gesture in _gestures)
                gesture.Update(body);
        }

        public void AddGesture(Type type)
        {
            // Check whether the gesure is already added.
            if (_gestures.Any(g => g.Type == type)) return;

            ISegment[] segments = null;

            switch (type)
            {
                case Type.SwipeLeft:
                    break;
                case Type.SwipeRight:
                    break;
                case Type.WaveRight:
                    segments = new ISegment[6];
                    var waveRightSegment1 = new FirstWaveSegment();
                    var waveRightSegment2 = new SecondWaveSegment();

                    segments[0] = waveRightSegment1;
                    segments[1] = waveRightSegment2;
                    segments[2] = waveRightSegment1;
                    segments[3] = waveRightSegment2;
                    segments[4] = waveRightSegment1;
                    segments[5] = waveRightSegment2;
                    break;
                case Type.Clap:
                    segments = new ISegment[4];
                    var clapSegment1 = new FirstClapSegment();
                    var clapSegment2 = new SecondClapSegment();
                    segments[0] = clapSegment1;
                    segments[1] = clapSegment2;
                    segments[2] = clapSegment1;
                    segments[3] = clapSegment2;
                    break;
                case Type.ZoomIn:
                    break;
                case Type.ZoomOut:
                    break;
                default:
                    break;
            }

            if (segments != null)
            {
                var gesture = new Gesture(type, segments);
                gesture.GestureRecognized += OnGestureRecognized;

                _gestures.Add(gesture);
            }
        }

        private void OnGestureRecognized(object sender, GestureEventArgs e)
        {
            if (GestureRecognized != null)
                GestureRecognized(this, e);

            foreach (var gesture in _gestures)
                gesture.Reset();
        }
    }
}