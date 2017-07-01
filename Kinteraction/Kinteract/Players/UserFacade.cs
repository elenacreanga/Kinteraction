using System;
using System.Collections.Generic;
using Microsoft.Kinect;

namespace Kinteract.Players
{
    public class UserFacade : BaseFacade<IEnumerable<Body>>
    {
        private readonly UsersFacadeEventArgs _args = new UsersFacadeEventArgs();
        private int _consecutiveFrames;

        private HashSet<ulong> _trackingIds = new HashSet<ulong>();

        public event EventHandler<UsersFacadeEventArgs> UserLeft;

        public event EventHandler<UsersFacadeEventArgs> UserEntered;

        public override void Start()
        {
            base.Start();

            _consecutiveFrames = 0;
        }

        public override void Stop()
        {
            base.Stop();

            _consecutiveFrames = 0;
        }

        public override void Update(IEnumerable<Body> bodies)
        {
            base.Update(bodies);

            var trackingIds = new HashSet<ulong>();

            foreach (var body in bodies)
                if (body.IsTracked)
                    trackingIds.Add(body.TrackingId);

            var currentCount = trackingIds.Count;
            var previousCount = _trackingIds.Count;

            if (currentCount != previousCount)
            {
                _consecutiveFrames++;

                if (_consecutiveFrames >= WindowSize)
                {
                    // The users that entered or left.
                    var users = new HashSet<ulong>();

                    if (currentCount > previousCount)
                    {
                        // ONE OR MORE USERS ENTERED
                        foreach (var id in trackingIds)
                            if (!_trackingIds.Contains(id))
                                users.Add(id);

                        _args.Users = users;

                        UserEntered?.Invoke(this, _args);
                    }
                    else
                    {
                        // ONE OR MORE USERS LEFT
                        foreach (var id in _trackingIds)
                            if (!trackingIds.Contains(id))
                                users.Add(id);

                        _args.Users = users;

                        UserLeft?.Invoke(this, _args);
                    }

                    _trackingIds = trackingIds;
                    _consecutiveFrames = 0;
                }
            }
        }
    }
}