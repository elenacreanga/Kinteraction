using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;

namespace Kinteraction.Gestures
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