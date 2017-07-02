using System;
using Microsoft.Kinect;

namespace Kinteract.Gestures
{
    internal class Gesture
    {
        private const int MaxFramesPausedGestureCount = 10;
        private const int WindowSize = 50;

        private int _currentSegment;

        private int _frameCount;

        private bool _isPaused;

        private int _pausedFrameCount = 10;

        public Gesture()
        {
        }

        public Gesture(Type type, ISegment[] segments)
        {
            Type = type;
            Segments = segments;
        }

        public Type Type { get; set; }

        public ISegment[] Segments { get; set; }

        public event EventHandler<GestureEventArgs> GestureRecognized;

        public void Update(Body body)
        {
            if (_isPaused)
            {
                if (_frameCount == _pausedFrameCount)
                    _isPaused = false;

                _frameCount++;
            }

            var result = Segments[_currentSegment].Check(body);

            if (result == Outcome.Successful)
            {
                if (_currentSegment + 1 < Segments.Length)
                {
                    _currentSegment++;
                    _frameCount = 0;
                    _pausedFrameCount = MaxFramesPausedGestureCount;
                    _isPaused = true;
                }
                else
                {
                    if (GestureRecognized != null)
                    {
                        GestureRecognized(this, new GestureEventArgs(Type, body.TrackingId));
                        Reset();
                    }
                }
            }
            else if (result == Outcome.Failed || _frameCount == WindowSize)
            {
                Reset();
            }
            else
            {
                _frameCount++;
                _pausedFrameCount = MaxFramesPausedGestureCount / 2;
                _isPaused = true;
            }
        }

        public void Reset()
        {
            _currentSegment = 0;
            _frameCount = 0;
            _pausedFrameCount = MaxFramesPausedGestureCount / 2;
            _isPaused = true;
        }
    }

    public enum Type
    {
        SwipeRight = 1,
        SwipeLeft = 2,
        WaveRight = 3,
        WaveLeft = 4,
        ZoomIn = 5,
        ZoomOut = 6,
        Clap = 7,
        CrossedArms = 8,
        Surrender = 9,
        KickRight = 10,
        KickLeft = 11,
        LiftRightLeg = 12,
        LiftLeftLeg = 13,
        Jump = 14,
        Squat = 15
    }

    public enum Outcome
    {
        Successful,
        Failed,
        Undetermined
    }
}