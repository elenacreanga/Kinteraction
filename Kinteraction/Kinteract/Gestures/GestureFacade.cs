using System;
using System.Collections.Generic;
using System.Linq;
using Kinteract.Gestures.Segments;
using Microsoft.Kinect;

namespace Kinteract.Gestures
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
                    //segments = new ISegment[6];
                    //var clapSegment1 = new FirstClapSegment();
                    //var clapSegment2 = new SecondClapSegment();
                    //segments[0] = clapSegment1;
                    //segments[1] = clapSegment2;
                    //segments[2] = clapSegment1;
                    //segments[3] = clapSegment2;
                    //segments[4] = clapSegment1;
                    //segments[5] = clapSegment2;
                    break;
                case Type.ZoomIn:
                    //segments = new ISegment[3];

                    //segments[0] = new ZoomSegmentOne();
                    //segments[1] = new ZoomSegmentTwo();
                    //segments[2] = new ZoomSegmentThree();
                    break;
                case Type.ZoomOut:
                    //segments = new ISegment[3];

                    //segments[0] = new ZoomSegmentThree();
                    //segments[1] = new ZoomSegmentTwo();
                    //segments[2] = new ZoomSegmentOne();
                    break;
                case Type.WaveLeft:
                    break;
                case Type.CrossedArms:
                    segments = new ISegment[3];
                    var crossedArms = new CrossedArmsSegment();
                    segments[0] = crossedArms;
                    segments[1] = crossedArms;
                    segments[2] = crossedArms;
                    break;
                case Type.Surrender:
                    segments = new ISegment[3];
                    var surrender = new SurrenderSegment();
                    segments[0] = surrender;
                    segments[1] = surrender;
                    segments[2] = surrender;
                    break;
                case Type.KickLeft:
                    segments = new ISegment[2];
                    var leftKickSegmentOne = new KickLeftSegmentOne();
                    var leftKickSegmentTwo = new KickLeftSegmentTwo();
                    segments[0] = leftKickSegmentOne;
                    segments[1] = leftKickSegmentTwo;
                    break;
                case Type.KickRight:
                    segments = new ISegment[2];
                    var rightKickSegmentOne = new KickRightSegmentOne();
                    var rightKickSegmentTwo = new KickRightSegmentTwo();
                    segments[0] = rightKickSegmentOne;
                    segments[1] = rightKickSegmentTwo;
                    break;
                case Type.Jump:
                    segments = new ISegment[4];
                    var jumpSegmentOne = new JumpSegmentOne();
                    var jumpSegmentTwo = new JumpSegmentTwo();
                    var jumpSegmentThree = new JumpSegmentThree(jumpSegmentTwo);
                    segments[0] = jumpSegmentOne;
                    segments[1] = jumpSegmentTwo;
                    segments[2] = jumpSegmentThree;
                    segments[3] = jumpSegmentOne;
                    break;
                case Type.Squat:
                    segments = new ISegment[2];
                    var squatSegmentOne = new SquatSegmentOne();
                    var squatSegmentTwo = new SquatSegmentTwo();
                    segments[0] = squatSegmentOne;
                    segments[1] = squatSegmentTwo;
                    break;
                case Type.LiftRightLeg:
                    //segments = new ISegment[2];
                    //var liftRightLegSegmentOne = new LiftRightLegSegmentOne();
                    //var liftRightLegSegmentTwo = new LiftRightLegSegmentTwo();
                    //segments[0] = liftRightLegSegmentOne;
                    //segments[1] = liftRightLegSegmentTwo;
                    break;
                case Type.LiftLeftLeg:
                    //segments = new ISegment[2];
                    //var liftLeftLegSegmentOne = new LiftLeftLegSegmentOne();
                    //var liftLeftLegSegmentTwo = new LiftLeftLegSegmentTwo();
                    //segments[0] = liftLeftLegSegmentOne;
                    //segments[1] = liftLeftLegSegmentTwo;
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