using System;
using Microsoft.Kinect;

namespace Kinteraction.Poses.Gestures
{
    internal class Gesture
    {
        private readonly int MAX_FRAMES_PAUSED_GESTURE_COUNT = 10;
        private readonly int WINDOW_SIZE = 50;

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
                    _pausedFrameCount = MAX_FRAMES_PAUSED_GESTURE_COUNT;
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
            else if (result == Outcome.Failed || _frameCount == WINDOW_SIZE)
            {
                Reset();
            }
            else
            {
                _frameCount++;
                _pausedFrameCount = MAX_FRAMES_PAUSED_GESTURE_COUNT / 2;
                _isPaused = true;
            }
        }

        public void Reset()
        {
            _currentSegment = 0;
            _frameCount = 0;
            _pausedFrameCount = MAX_FRAMES_PAUSED_GESTURE_COUNT / 2;
            _isPaused = true;
        }
    }

    public enum Type
    {
        SwipeRight,
        SwipeLeft,
        WaveRight,
        WaveLeft,
        ZoomIn,
        ZoomOut
    }

    public enum Outcome
    {
        Successful,
        Failed,
        Undetermined
    }
}