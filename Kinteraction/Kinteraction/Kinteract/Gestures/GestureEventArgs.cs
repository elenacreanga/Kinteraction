using System;

namespace Kinteraction.Kinteract.Gestures
{
    public class GestureEventArgs : EventArgs
    {
        public Type Type { get; }

        public ulong TrackingId { get; }

        public GestureEventArgs()
        {
        }

        public GestureEventArgs(Type type, ulong trackingId)
        {
            Type = type;
            TrackingId = trackingId;
        }
    }
}
